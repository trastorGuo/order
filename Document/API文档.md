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
                "DETAIL_PRICE" 0,
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
    }],
    "CAPITATION":'',
    "COST":0
    
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
    }],
    "CAPITATION":'',
    "COST":0
}
````



### /api/Product/Reprint

```` js
{
  "Foods": [
    {
      "DETAIL_ID": "30A4F1ED9AE1474997EBF3D508418A75",
      "NUM": 1
    },
    {
      "DETAIL_ID": "2F37224ABB7B451691D2EBED526A025B",
      "NUM": 2
    }
  ],
  "Account":"jiang1",
  "User": "trastor",
  "OrderId": "B17D70F631934A31A2A804EB892B77CE",
  "DescNum":"1",
  "PersonNum":2,
}
````

### /api/user/ModifiefPassword?username={0}&passwordbefore={1}&passwordafter={2}

```` js
{
    "username":"",
    "passwordbefore":"",
	"passwordafter":""
}
//参数：用户名，原密码，修改后密码
//管理员修改密码不会校验原密码
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

### /api/Product/GetOrders?id={0}&datetime={1}&to{2}&userOrdered={3}

```` json
//获取订单
{
  "success": true,
  "code": 200,
  "data": [
    {
      "DATE": "2021-02-03T00:00:00",
      "ORDER": [
        {
          "ORDER_DATE": "2021-02-03T22:38:25.92",
          "USER_ORDER": "god",
          "ORDER_ID": "7199B44618924ACCB07AB5CA0D61FD2F",
          "IS_PRINT": "Y",
          "IS_CLOSE": "N",
          "PERSON_NUM": 2,
          "DESC_NUM": "4",
          "FOODS": [
            {
              "ORDER_DETAIL_ID": "1400BDF443D74B1DA9CEE4C88A288EB9",
              "QTY": 1,
              "USER_ORDER": "god",
              "FOOD_DETAIL_NAME": "食物C",
              "PRICE": 45,
              "URLS":[]
            },
            {
              "ORDER_DETAIL_ID": "1400BDF443D74B1DA9CEE4C88A288EB9",
              "QTY": 3,
              "USER_ORDER": "god",
              "FOOD_DETAIL_NAME": "食物d(食物B_2)",
              "PRICE": 101,
              "URLS":[]
            },
            {
              "ORDER_DETAIL_ID": "1400BDF443D74B1DA9CEE4C88A288EB9",
              "QTY": 1,
              "USER_ORDER": "god",
              "FOOD_DETAIL_NAME": "222(1)",
              "PRICE": 0,
              "URLS":[]
            }
          ]
        },
        {
          "ORDER_DATE": "2021-02-03T22:38:34.603",
          "USER_ORDER": "god",
          "ORDER_ID": "A1FE7A4924984D82B1B5DD3F12749320",
          "IS_PRINT": "Y",
          "IS_CLOSE": "N",
          "PERSON_NUM": 6,
          "DESC_NUM": "4",
          "FOODS": [
            {
              "ORDER_DETAIL_ID": "535956991CA94465B008B57F2DF7C545",
              "QTY": 1,
              "USER_ORDER": "god",
              "FOOD_DETAIL_NAME": "222(1)",
              "PRICE": 0,
              "URLS":[]
            },
            {
              "ORDER_DETAIL_ID": "535956991CA94465B008B57F2DF7C545",
              "QTY": 1,
              "USER_ORDER": "god",
              "FOOD_DETAIL_NAME": "食物d(食物B_2)",
              "PRICE": 101,
              "URLS":[]
            },
            {
              "ORDER_DETAIL_ID": "535956991CA94465B008B57F2DF7C545",
              "QTY": 3,
              "USER_ORDER": "god",
              "FOOD_DETAIL_NAME": "食物C",
              "PRICE": 45,
              "URLS":[]
            }
          ]
        }
      ]
    }
  ]
}
````

### /api/Product/DeleteDesk?descNum={0}

```` json
//删除桌号
````

### /api/Product/DeleteOrder?id={0}

```` js
//参数：订单ID
````

### /api/User/BusinessStatus

```` js
//获取图标信息
//返回
salesToday,
salesMonth,
salesHalfYear,
week
````

