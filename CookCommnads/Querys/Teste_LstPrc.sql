DECLARE @Pai varchar(15) ='EMP1179'
DECLARE @Almoxarifado varchar(2)='01'
DECLARE @Filial varchar(4)='1002'
declare @CustoProd float
declare @CustoGrp float
declare @Estrutura float

  EXECUTE [dbo].Usp_ListaPrc
   @Pai
  ,@Almoxarifado
  ,@Filial  
  ,@CustoProd out
  ,@CustoGrp out
  ,@Estrutura out
  
  select Filial,B1_CC,B1_GRUPO,	Origem,	Id	,Nivel,	ProdutoInicial,	Produto,	Componente, B1_ORIGEM,	TipoComponente	,Unidade	,RendTeorico	
 from ##ListaPrc
	JOIN SB1010 ON B1_COD=Componente and B1_FILIAL=@Filial
  go
  drop table ##ListaPrc

  --cea047
--FOA390    

