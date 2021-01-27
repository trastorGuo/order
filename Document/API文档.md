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
//有TOKEN认证
````

### /api/Product/AddType

````json
{
    "SEQ":1,
    "ICON": "",
    "TYPE_NAME": ""
}
//有TOKEN认证
````

### /api/Product/EditType

```` json
{
    "SEQ":1,
    "ICON": "",
    "TYPE_NAME": ""
}
//有TOKEN认证
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
//有TOKEN认证
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
  "DescNum":"12" //桌号，第一次必传，子订单可不传
  "PersonNum":2//人数
  "IsPrint":"N"//是否打印，N,Y。默认Y
}
//无TOKEN认证
````

### /api/User/AddShop

```` json
//新增值允许管理员
//有TOKEN认证
{
    "NAME":"",
    "ADDRESS":"",
    "ACCOUNT":"",
    "PASSWORD":"",
    "TEL":"",
    "PRINTER":"",
    "URLS":[{
        "URL":""
    }]
}
````

### /api/User/EditShop

```` json
//编辑允许用户、管理员编辑
//有TOKEN认证
{
    "NAME":"",
    "ADDRESS":"",
    "ACCOUNT":"",//必填，为当前编辑的用户
    "PASSWORD":"",
    "TEL":"",
    "PRINTER":"",
    "URLS":[{
        "ID":"",//有ID修改，无ID新增
        "URL":""
    }]
}
````









## GET

### /api/Product/DeskIsOccupied?desckNum={0}&shopAcount={1}

```` json
//用户扫码先访问此API，判断此桌是否被占用，if 被占用，显示 新开一单 / 继续下单  两个按钮; else 直接显示菜单列表
//判断当前桌是否有人正在占用 true:被占用
//无TOKEN认证
````

### /api/Product/CloseOrder?orderId={0}

````json
//关闭订单
//无TOKEN认证
````

### /api/Product/GetDeskList

```` json
//获取当前店铺所有桌
//有TOKEN认证
````

### /api/Product/AddOrEditDesk?deskNum={0}&deskDesc={1}

```` json
//编辑或新增桌
//有TOKEN认证
````


### /api/User/ShopInfo?account={0}

```` json
//返回格式
//允许用户、和管理员访问
//有TOKEN认证
{
    "NAME":"",
    "ADDRESS":"",
    "ACCOUNT":"",
    "PASSWORD":"",
    "TEL":"",
    "PrinterCode":"",
    "URLS":[{
        "ID":"",
        "URL":""
    }]
}
````

### /api/Product/DeleteProduct?id={0}

```` json
//删除明细和Food都用此API
````

### /api/Printer/AddPrinter?snslist={0}

````json
//添加打印机
````

### /api/Product/DeleteType?id={0}

````json
//删除类别
````

