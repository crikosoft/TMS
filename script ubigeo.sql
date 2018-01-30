select * from OrdenEstadoOrdens

select * from Ordens


select * from ubigeos
where substring(codigo,3,4) = '0101' 
order by 2 


select * from ubigeos
where substring(codigo,1,2) = '10' and substring(codigo,3,2) <> '00' and substring(codigo,5,2) = '01'

select * from ubigeos
where substring(codigo,1,4) = '1001'


select * from ubigeos
where substring(codigo,1,4) = '1001'


select * from ubigeos
where descripcion like '%huanuco%'