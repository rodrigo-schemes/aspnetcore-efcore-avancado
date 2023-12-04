## Escopo do Curso

***
### Módulo 01
- Introdução
***
### Módulo 02
- Setup
***
### Módulo 03 - EF Database
- Ensure Created/Deleted
- GAP do Ensure Created em Múltiplos Contextos
- Healthcheck de Banco de Dados
- Gerenciar estado de conexão
- Comando ExecuteSQL
- Proteger de SQL Injection
- Migrações pendentes
- Forçando migração
- Recuperando migrações existentes
- Recuperando migrações aplicadas no DB
- Gerar Script SQL
***
### Módulo 04 - Tipos de Carregamento
- Adiantado
- Explícito
- Lento (LazyLoad)
***
### Módulo 05 - Tipos de Carregamento
- Configurando um filtro global
- Ignorando filtros globais
- Consultas projetadas
- Consulta interpolada
- Usando o recurso TAG em consultas
- Diferença em consulta 1xN vs Nx1
- Divisão de consultas em SplitQuery
***
### Módulo 06 - Stored Procedures
- Criando uma procedure de inserção
- Executando uma procedure de inserção
- Criando uma procedure de consulta
- Executando uma procedure de consulta
***
### Módulo 07 - Infraestrutura
- Configurando log simplificado
- Filtrar eventos de log
- Escrever log em arquivo
- Habilitar erros detalhados
- Habilitar visualização de dados sensíveis
- Configurar o batch size
- Configurar o timeout do comando global
- Configurar o timeout do comando para um fluxo
- Configurar resiliência
- Criar uma estratégia de resiliência
***
### Módulo 08 - Modelo de Dados
- Collations
- Sequences
- Índices
- Propagação de dados
- Esquemas
- Conversor de Valores
- Conversor de Valor customizado
- Shadow Property
- Owned Types
- Relacionamento 1 x 1
- Relacionamento 1 x N
- Relacionamento N x N
- Backing Field
- TPH e TPT (Herança)
- Property Bags
***
### Módulo 09 - Data Annotations
- Table, Key, Column
- Inverse Property
- NotMapped
- Database Generated
- Index
- Comment
- Backing Field
- Keyless
***
### Módulo 10 - EF Functions
- Funções de datas
- Like
- DataLength
- Property
- Collate
***
### Módulo 11 - Interceptação
- Criar e registrar um interceptador de comando
- Sobreescrever métodos das classes bases
- Aplicar hints em consultas
- Interceptar abertura de conexão com o banco
- Interceptar alterações
***
### Módulo 12 - Transações
- Comportamento padrão do EF Core
- Gerenciando transação manualmente
- Revertendo uma transação
- Salvando ponto de uma transação
- Usando TransactionScope
***
### Módulo 13 - UDF (User Defined Function)
- Built-In Function
- Registrar funções via Data Annotations
- Registrar funções via Fluent API
- Função definida pelo usuário
- Customizando uma função
***
### Módulo 14 - Performance
- Tracking vs NoTracking
- Resolução de Identidade
- Desabilitar rastreamento de consultas
- Consulta com tipo anônimo rastreada
- Consultas projetadas
***
### Módulo 15 - Migrações
- Migrations
- Gerar migration
- Gerar Script SQL
- Gerar Script SQL Idempotente
- Aplicar Migration
- Desfazer Migration
- Migration Pendentes
- Engenharia Reversa
***
### Módulo 16 - Banco de Dados
- PostgreSQL
- SQLite
- InMemory
- Azure Cosmos DB
***
### Módulo 17 - Multi-Tenant
- Arquitetura Multi-Tenant
- Single-tenant vs Multi-Tenant
- Estratégias Multi-Tenant 
  - Banco de Dados (Aderente a LGPD)
  - Schema
  - Tabela
***
### Módulo 18 - Padrão Repository & Unit Of Work
- Implementar persistência na API
- Implementar Repository Pattern
- Implementar UoW
- Repositório Genérico
- Consulta com respositório genérico
***
### Módulo 19 - Dicas e Truques
- Conhecer o método ToQueryString
- Depuração com Debug View
- Redefinir o estado do Contexto
- Include com consultas filtradas
- SingleOrDefault vs FirstOrDefault
- Tabela sem chave primária
- Usando Views de seu Banco de Dados
- Forçar o uso do VARCHAR
- Aplicar conversão de nomenclatura
- Operadores de Agregação
- Operadores de Agregação no agrupamento
- Contadores de Evento
***
### Módulo 20 - Testes
- Testes InMemory
- Testes com SQLite
***
### Módulo 21 - Sobreescrever comportamentos
- Gerador de SQL Customizado
***
### Módulo 22 - Diagnostics
- Diagnostic Source
- Interceptador
- Listener