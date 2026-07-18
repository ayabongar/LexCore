import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ContentService } from './services/content.service';
import { ContentItem, CreateContentItemRequest } from './models/content-item';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
  providers: [ContentService]
})
export class App implements OnInit {
  items: ContentItem[] = [];
  filteredItems: ContentItem[] = [];
  loading = false;
  error: string | null = null;

  // Filter controls
  selectedLanguage = '';
  selectedStatus = '';

  // Form controls
  showForm = false;
  formData: CreateContentItemRequest = {
    externalId: '',
    title: '',
    language: 'en',
    status: 'draft',
    tags: [],
    body: ''
  };
  tagInput = '';

  // Edit mode
  editingId: string | null = null;

  languages = ['en', 'fr'];
  statuses = ['draft', 'published', 'archived'];

  constructor(private contentService: ContentService) { }

  ngOnInit() {
    this.loadItems();
  }

  loadItems() {
    this.loading = true;
    this.error = null;
    this.contentService.getItems(this.selectedLanguage || undefined, this.selectedStatus || undefined)
      .subscribe({
        next: (data) => {
          this.items = data;
          this.filteredItems = data;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load items: ' + (err.message || 'Unknown error');
          this.loading = false;
        }
      });
  }

  onFilterChange() {
    this.loadItems();
  }

  toggleForm() {
    this.showForm = !this.showForm;
    if (this.showForm) {
      this.resetForm();
    }
  }

  resetForm() {
    this.formData = {
      externalId: '',
      title: '',
      language: 'en',
      status: 'draft',
      tags: [],
      body: ''
    };
    this.tagInput = '';
    this.editingId = null;
  }

  addTag() {
    if (this.tagInput.trim()) {
      this.formData.tags.push(this.tagInput.trim());
      this.tagInput = '';
    }
  }

  removeTag(index: number) {
    this.formData.tags.splice(index, 1);
  }

  submitForm() {
    if (!this.formData.title || !this.formData.externalId) {
      this.error = 'Title and External ID are required';
      return;
    }

    this.loading = true;
    this.error = null;

    if (this.editingId) {
      this.contentService.updateItem(this.editingId, {
        title: this.formData.title,
        language: this.formData.language,
        status: this.formData.status,
        tags: this.formData.tags,
        body: this.formData.body
      }).subscribe({
        next: () => {
          this.loadItems();
          this.showForm = false;
          this.resetForm();
        },
        error: (err) => {
          this.error = 'Failed to update item: ' + (err.message || 'Unknown error');
          this.loading = false;
        }
      });
    } else {
      this.contentService.createItem(this.formData).subscribe({
        next: () => {
          this.loadItems();
          this.showForm = false;
          this.resetForm();
        },
        error: (err) => {
          this.error = 'Failed to create item: ' + (err.message || 'Unknown error');
          this.loading = false;
        }
      });
    }
  }

  editItem(item: ContentItem) {
    this.editingId = item.id;
    this.formData = {
      externalId: item.externalId,
      title: item.title,
      language: item.language,
      status: item.status,
      tags: [...item.tags],
      body: item.body
    };
    this.showForm = true;
  }

  deleteItem(id: string) {
    if (confirm('Are you sure you want to delete this item?')) {
      this.loading = true;
      this.contentService.deleteItem(id).subscribe({
        next: () => {
          this.loadItems();
        },
        error: (err) => {
          this.error = 'Failed to delete item: ' + (err.message || 'Unknown error');
          this.loading = false;
        }
      });
    }
  }
}
