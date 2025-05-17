/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     5/17/2025 2:00:46 PM                         */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BOOKING') and o.name = 'FK_BOOKING_CREATE2_RIDE')
alter table BOOKING
   drop constraint FK_BOOKING_CREATE2_RIDE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BOOKING') and o.name = 'FK_BOOKING_MAKE_USER')
alter table BOOKING
   drop constraint FK_BOOKING_MAKE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('DRIVERPHONE') and o.name = 'FK_DRIVERPH_HAS6_DRIVER')
alter table DRIVERPHONE
   drop constraint FK_DRIVERPH_HAS6_DRIVER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('FEEDBACK') and o.name = 'FK_FEEDBACK_GIVE_USER')
alter table FEEDBACK
   drop constraint FK_FEEDBACK_GIVE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('FEEDBACK') and o.name = 'FK_FEEDBACK_RECEIVES_TRIP')
alter table FEEDBACK
   drop constraint FK_FEEDBACK_RECEIVES_TRIP
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('HAS2') and o.name = 'FK_HAS2_HAS7_USER')
alter table HAS2
   drop constraint FK_HAS2_HAS7_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('HAS2') and o.name = 'FK_HAS2_HAS8_PROMOCOD')
alter table HAS2
   drop constraint FK_HAS2_HAS8_PROMOCOD
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('PAYMENT') and o.name = 'FK_PAYMENT_HAS2_RIDE')
alter table PAYMENT
   drop constraint FK_PAYMENT_HAS2_RIDE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('PAYMENT') and o.name = 'FK_PAYMENT_USEDBY2_PROMOCOD')
alter table PAYMENT
   drop constraint FK_PAYMENT_USEDBY2_PROMOCOD
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RIDE') and o.name = 'FK_RIDE_ASSIGNEDT_DRIVER')
alter table RIDE
   drop constraint FK_RIDE_ASSIGNEDT_DRIVER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RIDE') and o.name = 'FK_RIDE_REFERENCE_VEHICLE')
alter table RIDE
   drop constraint FK_RIDE_REFERENCE_VEHICLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RIDE') and o.name = 'FK_RIDE_REQUEST_USER')
alter table RIDE
   drop constraint FK_RIDE_REQUEST_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TRIP') and o.name = 'FK_TRIP_HAS4_RIDE')
alter table TRIP
   drop constraint FK_TRIP_HAS4_RIDE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('USERPHONE') and o.name = 'FK_USERPHON_HAS5_USER')
alter table USERPHONE
   drop constraint FK_USERPHON_HAS5_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('VEHICLE') and o.name = 'FK_VEHICLE_ASSIGNEDT_DRIVER')
alter table VEHICLE
   drop constraint FK_VEHICLE_ASSIGNEDT_DRIVER
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BOOKING')
            and   name  = 'MAKE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BOOKING.MAKE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BOOKING')
            and   name  = 'CREATE2_FK'
            and   indid > 0
            and   indid < 255)
   drop index BOOKING.CREATE2_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BOOKING')
            and   type = 'U')
   drop table BOOKING
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DRIVER')
            and   type = 'U')
   drop table DRIVER
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('DRIVERPHONE')
            and   name  = 'HAS6_FK'
            and   indid > 0
            and   indid < 255)
   drop index DRIVERPHONE.HAS6_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('DRIVERPHONE')
            and   type = 'U')
   drop table DRIVERPHONE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('FEEDBACK')
            and   name  = 'RECEIVES_FK'
            and   indid > 0
            and   indid < 255)
   drop index FEEDBACK.RECEIVES_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('FEEDBACK')
            and   name  = 'GIVE_FK'
            and   indid > 0
            and   indid < 255)
   drop index FEEDBACK.GIVE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FEEDBACK')
            and   type = 'U')
   drop table FEEDBACK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HAS2')
            and   name  = 'HAS8_FK'
            and   indid > 0
            and   indid < 255)
   drop index HAS2.HAS8_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('HAS2')
            and   name  = 'HAS7_FK'
            and   indid > 0
            and   indid < 255)
   drop index HAS2.HAS7_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('HAS2')
            and   type = 'U')
   drop table HAS2
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('PAYMENT')
            and   name  = 'USEDBY2_FK'
            and   indid > 0
            and   indid < 255)
   drop index PAYMENT.USEDBY2_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('PAYMENT')
            and   name  = 'HAS2_FK'
            and   indid > 0
            and   indid < 255)
   drop index PAYMENT.HAS2_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PAYMENT')
            and   type = 'U')
   drop table PAYMENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('PROMOCODE')
            and   type = 'U')
   drop table PROMOCODE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('RIDE')
            and   name  = 'ASSIGNEDTO_FK'
            and   indid > 0
            and   indid < 255)
   drop index RIDE.ASSIGNEDTO_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('RIDE')
            and   name  = 'REQUEST_FK'
            and   indid > 0
            and   indid < 255)
   drop index RIDE.REQUEST_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('RIDE')
            and   type = 'U')
   drop table RIDE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TRIP')
            and   name  = 'HAS4_FK'
            and   indid > 0
            and   indid < 255)
   drop index TRIP.HAS4_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TRIP')
            and   type = 'U')
   drop table TRIP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"USER"')
            and   type = 'U')
   drop table "USER"
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('USERPHONE')
            and   name  = 'HAS5_FK'
            and   indid > 0
            and   indid < 255)
   drop index USERPHONE.HAS5_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('USERPHONE')
            and   type = 'U')
   drop table USERPHONE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('VEHICLE')
            and   name  = 'ASSIGNEDTO2_FK'
            and   indid > 0
            and   indid < 255)
   drop index VEHICLE.ASSIGNEDTO2_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('VEHICLE')
            and   type = 'U')
   drop table VEHICLE
go

/*==============================================================*/
/* Table: BOOKING                                               */
/*==============================================================*/
create table BOOKING (
   BOOKINID             int                  not null,
   RIDEID               int                  not null,
   RID_USERID           int                  not null,
   DRIVERID             int                  not null,
   VEHICLEID            int                  not null,
   USERID               int                  not null,
   BOOKINGATDATE        datetime             not null,
   BOOKINGATTIME        datetime             not null,
   constraint PK_BOOKING primary key nonclustered (BOOKINID)
)
go

/*==============================================================*/
/* Index: CREATE2_FK                                            */
/*==============================================================*/
create index CREATE2_FK on BOOKING (
RIDEID ASC,
RID_USERID ASC,
DRIVERID ASC,
VEHICLEID ASC
)
go

/*==============================================================*/
/* Index: MAKE_FK                                               */
/*==============================================================*/
create index MAKE_FK on BOOKING (
USERID ASC
)
go

/*==============================================================*/
/* Table: DRIVER                                                */
/*==============================================================*/
create table DRIVER (
   DRIVERID             int                  not null,
   FIRSTNAME            varchar(20)          not null,
   LASTNAME             varchar(20)          null,
   LICENCENUMBER        varchar(16)          not null,
   STATUS               varchar(20)          not null,
   constraint PK_DRIVER primary key nonclustered (DRIVERID)
)
go

/*==============================================================*/
/* Table: DRIVERPHONE                                           */
/*==============================================================*/
create table DRIVERPHONE (
   PHONENUMBER          varchar(11)          not null,
   DRIVERID             int                  not null,
   constraint PK_DRIVERPHONE primary key nonclustered (PHONENUMBER, DRIVERID)
)
go

/*==============================================================*/
/* Index: HAS6_FK                                               */
/*==============================================================*/
create index HAS6_FK on DRIVERPHONE (
DRIVERID ASC
)
go

/*==============================================================*/
/* Table: FEEDBACK                                              */
/*==============================================================*/
create table FEEDBACK (
   FEEDBACKID           int                  not null,
   TRIP_ID              int                  not null,
   USERID               int                  not null,
   COMMENTS             varchar(100)         null,
   RATING               decimal(1,1)         null,
   SUBMITTEDATDATE      datetime             null,
   SUBMITTEDATTIME      datetime             null,
   constraint PK_FEEDBACK primary key nonclustered (FEEDBACKID, TRIP_ID, USERID)
)
go

/*==============================================================*/
/* Index: GIVE_FK                                               */
/*==============================================================*/
create index GIVE_FK on FEEDBACK (
USERID ASC
)
go

/*==============================================================*/
/* Index: RECEIVES_FK                                           */
/*==============================================================*/
create index RECEIVES_FK on FEEDBACK (
TRIP_ID ASC
)
go

/*==============================================================*/
/* Table: HAS2                                                  */
/*==============================================================*/
create table HAS2 (
   USERID               int                  not null,
   PROMOCODEID          int                  not null,
   NOOFUSE              int                  null,
   constraint PK_HAS2 primary key (USERID, PROMOCODEID)
)
go

/*==============================================================*/
/* Index: HAS7_FK                                               */
/*==============================================================*/
create index HAS7_FK on HAS2 (
USERID ASC
)
go

/*==============================================================*/
/* Index: HAS8_FK                                               */
/*==============================================================*/
create index HAS8_FK on HAS2 (
PROMOCODEID ASC
)
go

/*==============================================================*/
/* Table: PAYMENT                                               */
/*==============================================================*/
create table PAYMENT (
   PAYMENTID            int                  not null,
   PROMOCODEID          int                  null,
   RIDEID               int                  not null,
   USERID               int                  not null,
   DRIVERID             int                  not null,
   VEHICLEID            int                  not null,
   PAYMENT_METHOD       varchar(20)          not null,
   AMOUNT               decimal              not null,
   PAYMENT_STATUS       varchar(20)          not null,
   constraint PK_PAYMENT primary key nonclustered (PAYMENTID)
)
go

/*==============================================================*/
/* Index: HAS2_FK                                               */
/*==============================================================*/
create index HAS2_FK on PAYMENT (
RIDEID ASC,
USERID ASC,
DRIVERID ASC,
VEHICLEID ASC
)
go

/*==============================================================*/
/* Index: USEDBY2_FK                                            */
/*==============================================================*/
create index USEDBY2_FK on PAYMENT (
PROMOCODEID ASC
)
go

/*==============================================================*/
/* Table: PROMOCODE                                             */
/*==============================================================*/
create table PROMOCODE (
   PROMOCODEID          int                  not null,
   STARTDATE            datetime             not null,
   ENDDATE              datetime             null,
   DESCRIPTION          varchar(100)         null,
   CODE                 varchar(20)          not null,
   DISCOUNTAMOUNT       money                not null,
   ISACTIVE             bit                  not null,
   USAGELIMIT           int                  not null,
   constraint PK_PROMOCODE primary key nonclustered (PROMOCODEID)
)
go

/*==============================================================*/
/* Table: RIDE                                                  */
/*==============================================================*/
create table RIDE (
   RIDEID               int                  not null,
   USERID               int                  not null,
   DRIVERID             int                  not null,
   VEHICLEID            int                  not null,
   DEOPOFF_STREET       varchar(50)          not null,
   DROPOFF_CITY         varchar(50)          not null,
   PICKUP_STREET        varchar(50)          not null,
   PICKUP_CITY          varchar(50)          not null,
   RIDESTATUS           varchar(20)          not null,
   REQUESTEDTIME        datetime             not null,
   FINALFARE            money                null,
   ESTIMATEDFARE        money                null,
   constraint PK_RIDE primary key nonclustered (RIDEID, USERID, DRIVERID, VEHICLEID)
)
go

/*==============================================================*/
/* Index: REQUEST_FK                                            */
/*==============================================================*/
create index REQUEST_FK on RIDE (
USERID ASC
)
go

/*==============================================================*/
/* Index: ASSIGNEDTO_FK                                         */
/*==============================================================*/
create index ASSIGNEDTO_FK on RIDE (
DRIVERID ASC
)
go

/*==============================================================*/
/* Table: TRIP                                                  */
/*==============================================================*/
create table TRIP (
   TRIP_ID              int                  not null,
   RIDEID               int                  not null,
   USERID               int                  not null,
   DRIVERID             int                  not null,
   VEHICLEID            int                  not null,
   RIDE_TIME            datetime             null,
   RIDE_FEES            decimal              not null,
   constraint PK_TRIP primary key nonclustered (TRIP_ID)
)
go

/*==============================================================*/
/* Index: HAS4_FK                                               */
/*==============================================================*/
create index HAS4_FK on TRIP (
RIDEID ASC,
USERID ASC,
DRIVERID ASC,
VEHICLEID ASC
)
go

/*==============================================================*/
/* Table: "USER"                                                */
/*==============================================================*/
create table "USER" (
   USERID               int                  not null identity(1,1),
   FIRSTNAME            varchar(20)          not null,
   LASTNAME             varchar(20)          null,
   EMAIL                varchar(50)          not null,
   DEFAULTPAYMENTMETHOD varchar(50)          null,
   PASSWORD             varchar(50)          not null,
   constraint PK_USER primary key nonclustered (USERID)
)
go

/*==============================================================*/
/* Table: USERPHONE                                             */
/*==============================================================*/
create table USERPHONE (
   PHONENUMBER2         varchar(11)          not null,
   USERID               int                  not null,
   constraint PK_USERPHONE primary key nonclustered (PHONENUMBER2, USERID)
)
go

/*==============================================================*/
/* Index: HAS5_FK                                               */
/*==============================================================*/
create index HAS5_FK on USERPHONE (
USERID ASC
)
go

/*==============================================================*/
/* Table: VEHICLE                                               */
/*==============================================================*/
create table VEHICLE (
   VEHICLEID            int                  not null,
   DRIVERID             int                  null,
   VEHICLE_TYPE         varchar(20)          not null,
   PLATE_NUMBER         varchar(20)          not null,
   constraint PK_VEHICLE primary key nonclustered (VEHICLEID)
)
go

/*==============================================================*/
/* Index: ASSIGNEDTO2_FK                                        */
/*==============================================================*/
create index ASSIGNEDTO2_FK on VEHICLE (
DRIVERID ASC
)
go

alter table BOOKING
   add constraint FK_BOOKING_CREATE2_RIDE foreign key (RIDEID, RID_USERID, DRIVERID, VEHICLEID)
      references RIDE (RIDEID, USERID, DRIVERID, VEHICLEID)
go

alter table BOOKING
   add constraint FK_BOOKING_MAKE_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table DRIVERPHONE
   add constraint FK_DRIVERPH_HAS6_DRIVER foreign key (DRIVERID)
      references DRIVER (DRIVERID)
go

alter table FEEDBACK
   add constraint FK_FEEDBACK_GIVE_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table FEEDBACK
   add constraint FK_FEEDBACK_RECEIVES_TRIP foreign key (TRIP_ID)
      references TRIP (TRIP_ID)
go

alter table HAS2
   add constraint FK_HAS2_HAS7_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table HAS2
   add constraint FK_HAS2_HAS8_PROMOCOD foreign key (PROMOCODEID)
      references PROMOCODE (PROMOCODEID)
go

alter table PAYMENT
   add constraint FK_PAYMENT_HAS2_RIDE foreign key (RIDEID, USERID, DRIVERID, VEHICLEID)
      references RIDE (RIDEID, USERID, DRIVERID, VEHICLEID)
go

alter table PAYMENT
   add constraint FK_PAYMENT_USEDBY2_PROMOCOD foreign key (PROMOCODEID)
      references PROMOCODE (PROMOCODEID)
go

alter table RIDE
   add constraint FK_RIDE_ASSIGNEDT_DRIVER foreign key (DRIVERID)
      references DRIVER (DRIVERID)
go

alter table RIDE
   add constraint FK_RIDE_REFERENCE_VEHICLE foreign key (VEHICLEID)
      references VEHICLE (VEHICLEID)
go

alter table RIDE
   add constraint FK_RIDE_REQUEST_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table TRIP
   add constraint FK_TRIP_HAS4_RIDE foreign key (RIDEID, USERID, DRIVERID, VEHICLEID)
      references RIDE (RIDEID, USERID, DRIVERID, VEHICLEID)
go

alter table USERPHONE
   add constraint FK_USERPHON_HAS5_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table VEHICLE
   add constraint FK_VEHICLE_ASSIGNEDT_DRIVER foreign key (DRIVERID)
      references DRIVER (DRIVERID)
go

