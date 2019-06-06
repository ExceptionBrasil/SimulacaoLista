          
ALTER Procedure  Usp_ListModel          
 (             
  @ProdutoInicial as varchar(15),    
  @Almoxarifado varchar(02),            
  @Filial varchar(4),              
  @RevisaoEscolhida varchar(03)=''        
 )            
            
As            
            
          
/*            
** Autor: Daniel P silveira            
** Data : 03/09/2018            
** Cliente: Colorobbia             
** Descri��o: Proc com a finalidade de gerar a estrutura de produtos, 
	com os custos de mat�ria prima, CM, custos ajustados por grupo e produto
*/            
            
--------------------------------------------  
--------------------------------------------            
-- Cria a Tabela Tempor�ria            
--------------------------------------------  
--------------------------------------------  
CREATE TABLE #ListModel(      
   Id int identity not null primary key,           
  Processado bit,          
  Filial Varchar(4) COLLATE Latin1_General_BIN,          
  ProdutoInicial  VARCHAR(15) COLLATE Latin1_General_BIN,          
  DescProdutoInicial  Varchar(80) Collate Latin1_General_BIN,          
  Origem  int default (0),          
  Produto  VARCHAR(15) COLLATE Latin1_General_BIN,          
  DescricaoProduto  VARCHAR(80) COLLATE Latin1_General_BIN,           
  Componente  VARCHAR(15) COLLATE Latin1_General_BIN,          
  DescricaoComponente  VARCHAR(80) COLLATE Latin1_General_BIN,          
  TipoComponente varchar(2) COLLATE Latin1_General_BIN,          
  Unidade varchar(2) COLLATE Latin1_General_BIN,       
  RendTeorico FLOAT  default 0,          
  RendReal float  default 0,           
  UltimoCustoMedio FLOAT  default 0,            
  CustoUnit float default 0,           
  Nivel int,          
  CCProduto varchar(9) COLLATE Latin1_General_BIN,          
  CComponente varchar(9) COLLATE Latin1_General_BIN,                 
  CusFixGrp float default 0,            
  CusFixGrpCalc float default 0,  
  CusMP float default 0,      
  DespesaOP float default 0,     
  CusMPCalc float default 0     
 )      
 --Cria um �ndice de performace       
 create index Ix_ExplosionBulk_001 on #ListModel (Filial,ProdutoInicial,Componente,Nivel)       
--------------------------------------------  
--------------------------------------------  
  
               
 ------------------------------------------------------------  
 ------------------------------------------------------------               
  
 -- INICIALIZAR TODAS AS VARI�VEIS NUM�RICAS COM ZERO  
   
 ------------------------------------------------------------  
 ------------------------------------------------------------  
                
 --Vari�veis Tempor�rias que podem mudar a qualquer instante                
      
 declare   @Id int                         
 declare   @Rendimento float=0                        
 declare   @CustoDoProduto float=0   
 declare   @DespesaOperacional float =0  
                
 declare   @Origem int                
 declare   @Componente  Varchar(15)  
 declare   @Revis�oAtual varchar(12)                
 declare   @Revis�oVld   int=0  
                
 declare @GrupoPai varchar(4) =  (select B1_GRUPO from SB1010 WHERE B1_COD=@ProdutoInicial and B1_FILIAL=@Filial AND D_E_L_E_T_='')     
 declare @GrupoComponente varchar(4) -- grupo do componente                
 declare @GrupoProduto varchar(4) -- Grupo do Produto  
                
 declare @CusFixGrp  float =0  
 declare @CusMP float =0           
                
 --Vari�veis SAGRADAS da explosao para montar os n�vels do produto                
 Declare @Continua bit =1    --Usado para verificar se ainda h� produtos PA dentro da estrutura para explodir                
 declare   @NextProduto varchar(15)=@ProdutoInicial  --Proximo produto do tipo PA da estrutura, inicializa com o produto Pai                  
 declare   @Nivel int =1      --Inicializa sempre com valor 1                
 declare   @Produto varchar(15)=@ProdutoInicial   --Produto Inicial que est� sofrento atualiza��o,                
 declare   @TipoProduto varchar(2)                
 declare   @TipoComponente varchar(2)                
             
         
 ------------------------------------------------------------
 ------------------------------------------------------------

  -- Monta a estrutura de produto               
  -- Ela � retroalimentada a previs�o do n�vel anteriror para 
  -- fazer a cascata do produzido te�rico                 
 
 ------------------------------------------------------------
 ------------------------------------------------------------        
      
            
while (@Continua=1)            
begin            
             

    
-----------------------------------------          
--Obtem a revis�o atual            
-----------------------------------------          
       
 SET @Revis�oAtual=(SELECT case B1_REVATU when '' then null else B1_REVATU end            
   FROM SB1010 WHERE B1_COD=@NextProduto AND B1_FILIAL=@Filial AND B1_REVATU<>'' AND D_E_L_E_T_='')              
            
            
 if  @Revis�oAtual is null             
 BEGIN            
  SET @Revis�oAtual=(SELECT MAX(G1_REVFIM) FROM SG1010             
  WHERE G1_COD=@NextProduto AND D_E_L_E_T_='' AND G1_FILIAL=@Filial and G1_REVFIM NOT IN ('ZZZ',''))            
 END       
 
            
 if  @Revis�oAtual is null             
 BEGIN            
  SET @Revis�oAtual=(SELECT MAX(G1_REVFIM) FROM SG1010             
  WHERE G1_COD=@NextProduto AND D_E_L_E_T_='' AND G1_FILIAL=@Filial and G1_REVFIM NOT IN (''))            
 END       
   
   --Valida se a revisao do produto est� igual que h� na estrutura
   --Exemplo> B1_REVATU=001 e G1_REVFIM='ZZZ'
   select @Revis�oVld=Count(G1_REVFIM) 
   FROM SG1010 AS G1              
	 JOIN SB1010 AS B1 ON B1_COD=G1_COMP AND B1_FILIAL=G1_FILIAL    
   WHERE               
	  B1.B1_FILIAL=@Filial              
	  AND G1.G1_FILIAL=@Filial               
	  AND B1.D_E_L_E_T_=''              
	  AND G1.D_E_L_E_T_=''               
	  AND B1.B1_TIPO NOT IN ('GG','OI')              
	  AND G1_COD=@NextProduto              
	  AND G1_REVFIM = @Revis�oAtual            

	If @Revis�oVld=0
	begin
	  SET @Revis�oAtual=(SELECT MAX(G1_REVFIM) FROM SG1010             
	  WHERE G1_COD=@NextProduto AND D_E_L_E_T_='' AND G1_FILIAL=@Filial and G1_REVFIM NOT IN ('ZZZ',''))       
	End
	
	if  @Revis�oAtual is null             
	BEGIN            
		 SET @Revis�oAtual=(SELECT MAX(G1_REVFIM) FROM SG1010             
		WHERE G1_COD=@NextProduto AND D_E_L_E_T_='' AND G1_FILIAL=@Filial and G1_REVFIM NOT IN (''))            
	END 
       
  
If @RevisaoEscolhida<>''   AND  @NextProduto=@ProdutoInicial    
 Set @Revis�oAtual = @RevisaoEscolhida        


  -------------------------------------------  
  --Insere a estrutura na tabela de explosao          
  -------------------------------------------
   Insert Into #ListModel            
    ( Filial,            
     ProdutoInicial,               
     DescProdutoInicial,           
     Produto,           
     DescricaoProduto,                 
     Componente,            
     DescricaoComponente,    
  Unidade,            
     TipoComponente,            
     RendTeorico,               
     Nivel,    
     Origem          
    )            
            
   SELECT           
   G1_FILIAL ,            
     @ProdutoInicial,            
     B1_02.B1_DESC,    --Descri��o do Produto ProdutoInicial          
     G1_COD,    --Produto           
     B1_01.B1_DESC,    --descri��o do Produto           
     G1_COMP,    --Componente          
     B1.B1_DESC,       --Descricao do Componente   
  B1.B1_UM, --Uni         
     B1.B1_TIPO,   --tipo do Componente          
     G1_QUANT,   --F�rmula         
     @Nivel,     
     @Id          
          
   FROM SG1010 AS G1            
    JOIN SB1010 AS B1 ON B1_COD=G1_COMP AND B1_FILIAL=G1_FILIAL            
    JOIN SB1010 AS B1_01 ON B1_01.B1_COD=G1_COD AND B1_01.B1_FILIAL=G1_FILIAL            
    JOIN SB1010 AS B1_02 ON B1_02.B1_COD=@ProdutoInicial AND B1_02.B1_FILIAL=@Filial          
               
   WHERE             
      B1.B1_FILIAL=@Filial                
  AND B1.B1_TIPO NOT IN ('GG','OI')                                
  AND B1.D_E_L_E_T_=''                
  AND B1_01.D_E_L_E_T_=''                
  AND B1_02.D_E_L_E_T_=''                
  AND G1.G1_FILIAL=@Filial 
  AND G1.D_E_L_E_T_=''     
  AND G1_COD=@NextProduto                
  AND G1_REVFIM = @Revis�oAtual               
          
          
 --Marca que esse Componente j� processei dentro do produto ProdutoInicial e dentro do periodo atual          
 UPDATE #ListModel           
 Set Processado =1           
 where           
  Componente=@NextProduto            
 And TipoComponente='PA'          
 And Produto =@Produto          
 and ProdutoInicial=@ProdutoInicial           
 and Id = @Id   
  
           
            
 ------------------------------------------------------------
 ------------------------------------------------------------              
 
  -- Varre a estrutura Gerada atualizando com o c�lculo de               
  -- de producao e Rendimento               
                
 ------------------------------------------------------------
 ------------------------------------------------------------         
 --Monta o cursor de processamento            
 Declare Cur cursor for             
  Select             
   Id,TipoComponente,Origem,Componente,Produto            
  From #ListModel            
  Where                       
   Nivel=@Nivel            
   and ProdutoInicial=@ProdutoInicial            
            
             
 Open Cur            
            
             
 Fetch Next From Cur into @Id,@TipoComponente,@Origem,@Componente,@Produto            
            
 While @@FETCH_STATUS=0            
 Begin            
              
  --Quando o componete for PA atualiza os dados de rendimento e produ��o            
  If @TipoComponente='PA'            
  Begin            
            
   --Obtem a �ltima F�rmula (estrutura do componente)             
   SELECT @Revis�oAtual=B1_REVATU             
   FROM             
    SB1010             
   WHERE             
    B1_COD=@Produto             
   AND B1_FILIAL=@FILIAL             
   AND B1_REVATU<>''             
   AND D_E_L_E_T_=''             
            
   --Obtem o Rendimento do Componente quando ele for um PA            
   --Necessito do rendimento do Componente, para fazer o c�lculo de proje��o de produ��o            
   --para o pr�ximo n�vel            
   SET @Rendimento =100 --incializa o rendimento com 100 para que o primeiro n�vel fique correto            
            
   Select             
    @Rendimento = IsNull(Z01_REND,100)            
   From Z01010            
   Where             
    Z01_COD=@Produto            
    AND Z01_FORM=@Revis�oAtual            
    and D_E_L_E_T_=''              
                
               
   --Se o  rendimento for 100% OU SEJA n�o houve atualiza��o de rendimento logo acima            
   --Usa o rendimento do Produto             
   IF @Rendimento=100            
   Begin             
    --Obtem a �ltima F�rmula (estrutura do Produto)             
    SELECT @Revis�oAtual=B1_REVATU             
    FROM             
     SB1010             
    WHERE             
     B1_COD=@Produto             
    AND B1_FILIAL=@FILIAL             
    AND B1_REVATU<>''             
    AND D_E_L_E_T_=''             
            
    --Obtem o Rendimento do Produto quando ele for um PA            
    --Necessito do rendimento do Componente, para fazer o c�lculo de proje��o de produ��o            
    --para o pr�ximo n�vel            
    Select             
     @Rendimento = IsNull(Z01_REND,100)            
    From Z01010            
    Where             
     Z01_COD=@Produto            
     AND Z01_FORM=@Revis�oAtual            
     and D_E_L_E_T_=''            
   End            
            
                
            
  End             
  Else --- Quando o componente for difenrente de PA            
  Begin             
            
   --Obtem a �ltima F�rmula (estrutura do Produto)             
   SELECT @Revis�oAtual=B1_REVATU             
   FROM             
    SB1010             
   WHERE             
    B1_COD=@Produto             
   AND B1_FILIAL=@FILIAL             
   AND B1_REVATU<>''             
   AND D_E_L_E_T_=''             
            
   --Obtem o Rendimento do Produto quando ele for um PA            
   --Necessito do rendimento do Componente, para fazer o c�lculo de proje��o de produ��o            
   --para o pr�ximo n�vel            
   Select             
    @Rendimento = IsNull(Z01_REND,100)            
   From Z01010            
   Where             
    Z01_COD=@Produto            
    AND Z01_FORM=@Revis�oAtual            
    and D_E_L_E_T_=''            
  End              
              
  --Iguala o primeiro n�vel a 100%              
  if @Nivel=1             
   begin             
   update             
    #ListModel            
   set RendReal=100            
               
   where             
    Id = @Id            
  end            
            
  ----            
  --Neste ponto faz ajuste do valor de rendimento  e calcula a Producao Te�rica para o n�vel 1            
  ----            
            
  --tratativa especifica para colorobbia quando for FRITA de 1 n�vel s�            
  if @Nivel=1 AND left(@GrupoPai,2)='FR'             
   begin             
   update             
    #ListModel            
   set RendReal=@Rendimento  
     
   where             
    Id = @Id            
  end            
            
  ---            
  -- calcula a Producao Te�rica para os demais n�veis            
  ---            
  if @Nivel>1            
  begin             
   update            
     #ListModel            
    set RendReal=@Rendimento  
       
    where             
     Id = @Id            
  End               
  Fetch Next From Cur into @Id,@TipoComponente,@Origem,@Componente,@Produto            
 End            
            
 --Fecha e Desaloca o cursor            
 Close Cur            
 Deallocate Cur            
            
            
            
 --Obtem o pr�ximo produto PA da estrutura para processar            
 --E calcula o rendimento real para a previsao do proximo n�vel             
 Select             
  top 1 @NextProduto=Componente,            
     @Produto=Produto,             
     @Id = Id,            
     @TipoProduto=TipoComponente  
            
  from #ListModel               
  WHERE             
   TipoComponente='PA'             
  AND Processado is null                        
  and ProdutoInicial=@ProdutoInicial            
            
            
  --A partir do segundo n�vel na filial 1002               
  --troca a filial para 1001            
  If @TipoProduto='PA' and @Filial='1002'            
  begin             
   set @Filial='1001'            
  end            
            
            
            
            
 --Incrementa o n�vel da Estrutura            
 --Nivel da estrutura            
  set @Nivel= (select max(Nivel)+1             
    from #ListModel            
    where ProdutoInicial=@ProdutoInicial            
    )            
              
             
 --Verifica se ainda h� mais produtos para processar             
 IF( Select Count(*)             
   from #ListModel                
   WHERE             
   TipoComponente='PA'            
   AND Processado is null                    
   and ProdutoInicial=@ProdutoInicial) >0            
              
 BEGIN             
  SET @Continua=1            
 END            
 ELSE            
 BEGIN            
  SET @Continua=0            
 END            
end            
            

            
            
-----------------------------------------------------            
-- Atualiza com o centro de Custo do Produto             
--  e com do componente            
-----------------------------------------------------            
              
            
update #ListModel             
 Set CCProduto=B1_CC            
 from SB1010            
 where B1_COD=Produto             
  and B1_FILIAL=Filial            
  AND D_E_L_E_T_=''            
  and ProdutoInicial=@ProdutoInicial                   
            
            
update #ListModel             
 Set CComponente=B1_CC            
 from SB1010            
 where B1_COD=Componente             
  and B1_FILIAL=Filial            
  AND D_E_L_E_T_=''            
  and ProdutoInicial=@ProdutoInicial            
           
            
             
            
/*******************************************************************            
*******************************************************************            
** A PARTIR DESSE PONTO TODA A ESTRUTURA DO PRODUTO J� EST� GERADA             
** E IREI INICIAR OS C�LCULOS COMPLEMENTARES            
*******************************************************************            
*******************************************************************/            
               
 ----------------------------------------------------------------------------                
 ----------------------------------------------------------------------------                
     
 --    OBTEM OS VALORES DE CUSTOS. M�DIO E FIXOS                     
     
 ----------------------------------------------------------------------------                
 ----------------------------------------------------------------------------                        
  DECLARE Cur Cursor  for                 
  Select Id,TipoComponente,Origem,Componente,Produto,RendReal              
  from #ListModel                
  where                            
   ProdutoInicial=@ProdutoInicial       
      order by Nivel desc         
                
 Open Cur                
                
 Fetch Next from Cur Into @Id,@TipoComponente,@Origem,@Componente,@Produto,@Rendimento
                
 while @@FETCH_STATUS=0                
 Begin                 
            
  --CUSTO M�DIO--        
  --obtem o �ltimo custo v�lido DO COMPONENTE                
  SELECT TOP 1 @CustoDoProduto=ISNULL(B9_CM1,0)                
   FROM SB9010                 
   WHERE                 
  D_E_L_E_T_=''                 
    AND B9_FILIAL=@Filial                
    AND B9_LOCAL=@Almoxarifado                        
    AND B9_CM1>0                
    and B9_COD=@Componente                    
   group by B9_CM1,B9_DATA                
   order by B9_DATA DESC                
                
  --Busca o custo Fixo de Grupo e Produto    
  set @CusMP=0                
  --set @CusFixGrp=0                
  --set @Rendimento=100                
  --set @Revis�oAtual=''                
                 
         
   --Custo Materia Prima do produto--                
   SELECT @CusMP=ISNULL(Z06_CUSTO1,0)        
   FROM Z06010                
   WHERE Z06_PROD=@Componente      
   AND Z06_FILIAL=@Filial              
     
   --Busca o Custo pela NF  
   --Se o custo Fixo do Produto for zero, busca o �ltimo preco de compra, sem impostos          
   -- Bausca em todas as filiais, pois poss ter um produto que compro em uma filial sempre e apenas transfiro para outra filial
   	IF @CusMP=0    
	Begin     
		 Select @CusMP = (    
			Select  TOP 1    
			 (D1_CUSTO/D1_QUANT) as PrcUnitLiq    
			FROM SD1010 D1         
			WHERE     
			D1.D_E_L_E_T_=''     
			and D1_COD =@Componente    			
			AND D1_TIPO='N'    
			AND D1_CF IN ('1101','1102','2101','2102')  
			order by D1_DTDIGIT DESC    
			)    
	   End    
   
   

	 --Atualiza a Despesa Operacional	             
	Set @GrupoProduto     = (select B1_GRUPO from SB1010 WHERE B1_COD=@Produto and B1_FILIAL=@Filial AND D_E_L_E_T_='')   
	set @DespesaOperacional = ISNULL((SELECT Z07_CUSTO1 FROM Z07010 WHERE Z07_GRUPO=@GrupoProduto AND D_E_L_E_T_='' and Z07_FILIAL=@Filial),0)        				 
	UPDATE  #ListModel    SET DespesaOP	=@DespesaOperacional	 where Id=@Id    

	 
   --Atualiza o custo MP e M�dio para o registro atual  
   If (SELECT CusMP FROM  #ListModel   Where  Id=@Id ) <=0
   Begin
	  --Atualiza��o do registro                
	  UPDATE  #ListModel                 
		   SET UltimoCustoMedio		= ISNULL(@CustoDoProduto,0),                   
		   CustoUnit				= ISNULL(@CustoDoProduto*(RendTeorico/100),0), --f�rmula% x CM                               
		   CusMP					= IsNull(@CusMP,0),  		 
		   CusMPCalc				=0
		  
	  where Id=@Id    
	End


	--Atualiza o custo MP Calculado 
	Update #ListModel set CusMPCalc=CusMP*(RendTeorico/100) where Id=@Id

	--Atualiza na Origem o Custo MP, mesmo sendo um PA
		SET @CusMP = (SELECT (SUM(CusMPCalc)/(@Rendimento/100))+@DespesaOperacional FROM #ListModel WHERE Origem=@Origem)
		Update  #ListModel      
			Set  CusMP	=IsNull(@CusMP,0)
		Where Id=@Origem
		
		
                 
  Fetch Next from Cur Into @Id,@TipoComponente,@Origem,@Componente,@Produto,@Rendimento             
 End                 
                
 Close Cur                
 Deallocate Cur                
          
 
 
--Lista o resultado  
select * from #ListModel
order by ProdutoInicial,Nivel,Componente