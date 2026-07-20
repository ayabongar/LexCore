export interface ContentItem {
  id: string;
  externalId: string;
  title: string;
  language: string;
  status: string;
  tags: string[];
  publishedAt: string | null;
  body: string;
}

export interface CreateContentItemRequest {
  externalId: string;
  title: string;
  language: string;
  status: string;
  tags: string[];
  body: string;
}

export interface UpdateContentItemRequest {
  title: string;
  language: string;
  status: string;
  tags: string[];
  body: string;
}
