if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('MyRepos') and o.name = 'FK_MYREPOS_REFERENCE_ALLREPOS')
alter table MyRepos
   drop constraint FK_MYREPOS_REFERENCE_ALLREPOS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AllRepos')
            and   type = 'U')
   drop table AllRepos
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MyRepos')
            and   type = 'U')
   drop table MyRepos
go

/*==============================================================*/
/* Table: AllRepos                                              */
/*==============================================================*/
create table AllRepos (
   ID                   int                  not null,
   CodeNo               varchar(100)         not null,
   Name                 varchar(30)          null,
   FullName             varchar(50)          null,
   Content              text                 not null,
   GitUrl               varchar(500)         null,
   CreateTime           datetime             not null default getdate(),
   constraint PK_ALLREPOS primary key (ID)
)
go

/*==============================================================*/
/* Table: MyRepos                                               */
/*==============================================================*/
create table MyRepos (
   ID                   int                  not null,
   CreateTime           datetime             not null default getdate(),
   constraint PK_MYREPOS primary key (ID)
)
go

alter table MyRepos
   add constraint FK_MYREPOS_REFERENCE_ALLREPOS foreign key (ID)
      references AllRepos (ID)
go
