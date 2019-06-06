
DECLARE @RC int
DECLARE @Pai varchar(15) ='CEA039'
DECLARE @Almoxarifado varchar(2)='01'
DECLARE @Filial varchar(4)='1001'
DECLARE @RevisaoEscolhida varchar(3)=''

  EXECUTE [dbo].Usp_ListModel
   @Pai
  ,@Almoxarifado
  ,@Filial
  ,@RevisaoEscolhida 

