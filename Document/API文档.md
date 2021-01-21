[TOC]

## POST

###　/api/Product/AddProduct

```` json
{
    "Type": {
        "TYPE_ID": "",
        "ICON": null,
        "TYPE_NAME": ""
    },
    "Food": {
        "FOOD_ID": "",
        "FOOD_TAG": null,
        "FOOD_NAME": "",
        "FOOD_DETAIL": [
            {
                "DETAIL_ID": "",
                "DETAIL_NAME": "",
                "DETAIL_DESC": "",
                "DETAIL_PRICE": 0,
                "Urls": [
                    {
                        "IMG_ID": "",
                        "URL": ""
                    }
                ]
            }
        ],
        "Urls": [
            {
                "IMG_ID": null,
                "URL": null
            }
        ]
    }
}
````

### /api/Product/AddType

````json
{
    "ICON": "",
    "TYPE_NAME": ""
}
````

### /api/Product/EditProduct

```` json

````

### /api/Product/PlaceAnOrder

```` json
{
  "foods": [
    {
      "DETAIL_ID": "77A912C9F38E432C93ED246DA442A8BA",
      "NUM": 1
    },
    {
      "DETAIL_ID": "28130B36D899433A9DFDC59D0E493AA6",
      "NUM": 2
    }
  ],
  "account":"test01",
  "user": "trastor",
  "OrderId": "",//订单ID，第一次创建不传，子订单需要传
}
````

