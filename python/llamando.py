'''import urllib

link = "https://dummyjson.com/products/categories"
f = urllib.urlopen(link)
myfile = f.read()
print(myfile)'''


import requests
link = "https://dummyjson.com/products/categories"
f = requests.get(link)
print(f.json())