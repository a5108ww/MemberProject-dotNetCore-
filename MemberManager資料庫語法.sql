


Create DataBase MemberDB

use MemberDB

Create Table members(
id Bigint identity(1,1) Primary key not null,
[name] nvarchar(10) null,
mobileNumber nvarchar(10) null ,
birthday DateTime null,
loginAccount nvarchar(100) default '' not null,
loginPwd nvarchar(max) default '' not null,
removed bit not null
)

insert into members values('�¥@��','0981712610','1991-08-17','admin','12345',0)--1
insert into members values('�¥@��-�ϥΪ̱b��','0981712610','1991-08-17','member','12345',0)--2
insert into members values('�¥@��-�Ӯa�b��','0981712610','1991-08-17','business','12345',0)--3

Create Table sysRoles (
id Bigint identity(1,1) Primary key not null,
[name] nvarchar(100) default '' not null,
removed bit not null
)

insert into sysRoles values('Admin',0)--1 �����޲z��
insert into sysRoles values('Member',0)--2 �@��ϥΪ�
insert into sysRoles values('Business',0)--3 �Ӯa

Create Table memberRoles (
id Bigint identity(1,1) Primary key not null,
memberId Bigint not null,
sysRolesId Bigint not null,
removed bit not null
)

insert into memberRoles values(1,1,0)
insert into memberRoles values(2,2,0)
insert into memberRoles values(3,3,0)

Create Table sysFunctions (
id Bigint identity(1,1) Primary key not null,
parentId Bigint not null,
[displayName] nvarchar(100) default '' not null,
controllerName nvarchar(100) default '' not null,
actionName nvarchar(100) not null,
sort int not null,
removed bit not null
)

insert into sysFunctions values(0,'���~','Products','Index',1,0)
insert into sysFunctions values(0,'���~����','ProductTypes','Index',1,0)

Create Table sysRolesFunctions (
id Bigint identity(1,1) Primary key not null,
sysRoleId Bigint not null,
sysFunctionsId Bigint not null,
removed bit not null
)

insert into sysRolesFunctions values(1,1,0)
insert into sysRolesFunctions values(1,2,0)
insert into sysRolesFunctions values(2,1,0)
insert into sysRolesFunctions values(2,2,0)
insert into sysRolesFunctions values(3,1,0)
insert into sysRolesFunctions values(3,2,0)

Create Table [orders](
id Bigint identity(1,1) Primary key not null,
memberId Bigint not null,
orderStatusId int not null,
sendTypeId int not null,
[address] nvarchar(100) null,
phone nvarchar(20) null,
memo nvarchar(200) null,
createDate datetime not null,
removed bit not null
)

Create Table orderDetail (
id Bigint identity(1,1) Primary key not null,
productId int not null,
statusId int not null,
removed bit not null
)

Create Table productTypes (
id Bigint identity(1,1) Primary key not null,
[name] nvarchar(100) default '' not null,
sort int not null,
removed bit not null
)

insert into productTypes values('3C���~',1,0)
insert into productTypes values('�a�q',2,0)


Create Table products (
id Bigint identity(1,1) Primary key not null,
productTypeId Bigint not null,
[name] nvarchar(100) default '' not null,
price int not null,
sort int not null,
removed bit not null
)

insert into products values(1,'�Ūަվ�',1990,1,0)
insert into products values(2,'�L�i�l',5990,1,0)

Create Table sendTypes (
id Bigint identity(1,1) Primary key not null,
[name] nvarchar(100) default '' not null,
sort int not null,
removed bit not null
)

Create Table orderStatus (
id Bigint identity(1,1) Primary key not null,
[name] nvarchar(100) default '' not null,
sort int not null,
removed bit not null
)

Create Table orderDetailStatus (
id Bigint identity(1,1) Primary key not null,
[name] nvarchar(100) default '' not null,
sort int not null,
removed bit not null
)



