import json
import os

def normalize_items(input_path, output_path):
    """
    Reads a JSON Lines file, normalizes items into ContentItem shape,
    and handles duplicates based on externalId.
    """
    normalized_items = {}
    
    if not os.path.exists(input_path):
        print(f"Error: Input file {input_path} not found.")
        return

    with open(input_path, 'r', encoding='utf-8') as f:
        for line in f:
            if not line.strip():
                continue
            
            try:
                raw_item = json.loads(line)
                
                # Mapping fields
                ext_id = raw_item.get('ext_id') or raw_item.get('externalId')
                if not ext_id:
                    continue
                
                # Handle tags (could be string or list)
                tags = raw_item.get('tags', [])
                if isinstance(tags, str):
                    tags = [t.strip() for t in tags.split(',') if t.strip()]
                
                normalized = {
                    "externalId": ext_id,
                    "language": raw_item.get('lang') or raw_item.get('language', 'en'),
                    "title": raw_item.get('title', 'Untitled'),
                    "status": raw_item.get('status', 'draft'),
                    "tags": tags,
                    "publishedAt": raw_item.get('pub_date') or raw_item.get('publishedAt'),
                    "body": raw_item.get('body', '')
                }
                
                # Handle duplicates: keep the last one encountered
                normalized_items[ext_id] = normalized
                
            except json.JSONDecodeError as e:
                print(f"Error decoding line: {e}")

    # Convert dictionary back to list
    output_data = list(normalized_items.values())

    with open(output_path, 'w', encoding='utf-8') as f:
        json.dump(output_data, f, indent=4)
    
    print(f"Normalization complete. {len(output_data)} unique items written to {output_path}")

if __name__ == "__main__":
    input_file = "sample_input.jsonl"
    output_file = "normalized_output.json"
    normalize_items(input_file, output_file)
