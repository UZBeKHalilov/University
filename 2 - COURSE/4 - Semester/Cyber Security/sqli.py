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
USER_AGENT = ""


















