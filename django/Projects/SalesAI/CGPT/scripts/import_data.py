import pandas as pd
from django.contrib.auth.models import User
from CGPT.models import Item

csv_file_path = 'CGPT/Data/ItemList.csv'
df = pd.read_csv(csv_file_path)

def addItems():
    for index, row in df.iterrows():
        item = Item(
            itemId_int=row['ID'],
            itemName_text=row['Name'],
            itemInfo_text=row['Info'],
            itemPrice_int=row['CurrentPrice']
        )
        item.save()

def run():
    addItems()