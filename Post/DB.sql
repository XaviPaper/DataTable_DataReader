--drop database Post
create database Post
go
use Post
go
-- drop table Persona
create table Persona(
	Id int identity(1,1) primary key,
	Nombre nvarchar(max) not null,
	Apellidos nvarchar(max) not null,
	Nacimiento datetime,
	Direccion nvarchar(max) not null,
	Poblacion nvarchar(max) not null
)
go
declare @i int
set @i = 1
while (@i <= 10000)
begin
	insert Persona (Nombre, Apellidos, Nacimiento, Direccion, Poblacion) 
	values ('Nombre ' + cast(@i as nvarchar), 'Apellidos' + cast(@i as nvarchar), SYSDATETIME(), 'Dirección ' + cast(@i as nvarchar), 'Poblacion ' + cast(@i as nvarchar))

	set @i = @i + 1
end
go
select * from Persona