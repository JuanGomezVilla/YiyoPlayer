'''import urllib

link = "https://dummyjson.com/products/categories"
f = urllib.urlopen(link)
myfile = f.read()
print(myfile)'''


import requests
archivo = requests.get("https://raw.githubusercontent.com/JuanGomezVilla/YiyoPlayer/main/python/prueba.json")
enlaces = archivo.json()

for enlace in enlaces:
    print(enlace)