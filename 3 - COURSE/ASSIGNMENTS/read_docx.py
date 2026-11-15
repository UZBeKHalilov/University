import zipfile
import xml.etree.ElementTree as ET
import os

def get_docx_text(path):
    try:
        if not os.path.exists(path):
            return f"Error: File {path} not found"
        doc = zipfile.ZipFile(path)
        xml_content = doc.read('word/document.xml')
        root = ET.fromstring(xml_content)
        
        # Define namespaces
        ns = {'w': 'http://schemas.openxmlformats.org/wordprocessingml/2006/main'}
        
        paragraphs = []
        for p in root.findall('.//w:p', ns):
            texts = []
            for r in p.findall('.//w:r', ns):
                t = r.find('.//w:t', ns)
                if t is not None and t.text:
                    texts.append(t.text)
            paragraphs.append(''.join(texts))
        
        return '\n'.join(paragraphs)
    except Exception as e:
        return f"Error: {e}"

docx_path = "Abdulloh Halilov 23-310 Pitching and Negotiating Skills .docx"
text = get_docx_text(docx_path)
with open("current_content.txt", "w", encoding="utf-8") as f:
    f.write(text)
print("Done! Extracted text length:", len(text))
