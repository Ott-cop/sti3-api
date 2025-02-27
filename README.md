<h1>STi3 API</h1>

> <p>Desafio proposto da empresa STi3.</p>

<p>Backend feito em C# com .NET com arquitetura em camadas, para construção de um backend mais modular e flexível!</p>
<br/>
<h2>Configuração do arquivo .env</h2>
<p>Crie o arquivo .env na raiz do projeto usando o template de exemplo:</p>

    SERVER="localhost"
    DATABASE="sti3"
    USER="sti3"
    USER_PASSWORD="sti3123456@"
    
    EMAIL="youremailforbilling@email.com"
    BILLING="https://sti3-api-de-faturamento/api/vendas"
<br/>

<h2>Possíveis melhorias</h2>
<p>Uma das primeiras coisas que daria para melhorar, seria tornar o envio/verificação de pedidos pendentes rodar em paralelo. 
  Apesar do Serviço de Faturamento da aplicação rodar em segundo plano, as verificações/envios do pedido de faturamento não são.</p>
<p>Um dos benefícios disso seria que, em alta demanda de uso da API, os pedidos de faturamento que não conseguirem enviarem para o microserviço de faturamento por indisponibilidade não demorariam muito para realizar novamente as diversas requisições em paralelo.</p>
