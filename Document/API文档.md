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
{
    "Type": {
        "TYPE_ID": ""
    },
    "Food": {
        "FOOD_ID": "",
        "FOOD_TAG": "",
        "FOOD_NAME": "",
        "FOOD_DETAIL": [
            {
                "DETAIL_ID": "",
                "DETAIL_NAME": "",
                "DETAIL_DESC": "",
                "DETAIL_PRICE"0,
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
                "IMG_ID": "",
                "URL": ""
            }
        ]
    }
}
````

### /api/Product/PlaceAnOrder

```` json
{
  "Foods": [
    {
      "DETAIL_ID": "77A912C9F38E432C93ED246DA442A8BA",
      "NUM": 1
    },
    {
      "DETAIL_ID": "28130B36D899433A9DFDC59D0E493AA6",
      "NUM": 2
    }
  ],
  "Account":"test01",
  "User": "trastor",
  "OrderId": "",//订单ID，第一次创建不传，子订单需要传
  "DescNum":0 //桌号，第一次必传，子订单可不传
}
````

### /api/Product/DeskIsOccupied?desckNum={0}&shopAcount={1}

```` json
//用户扫码先访问此API，判断此桌是否被占用，if 被占用，显示 新开一单 / 继续下单  两个按钮; else 直接显示菜单列表
//判断当前桌是否有人正在占用 true:被占用
````

### /api/Product/CloseOrder?orderId={0}

````json
//关闭订单
````

