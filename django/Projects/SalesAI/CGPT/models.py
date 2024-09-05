from django.db import models
class Item(models.Model):
    itemId_int = models.IntegerField('item id')
    itemName_text = models.CharField(max_length=20)
    itemInfo_text = models.CharField(max_length=500)
    itemPrice_int = models.IntegerField('item price')
    def __str__(self):
        return self.itemName_text