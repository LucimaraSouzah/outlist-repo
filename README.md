# 🚫 Outlist - Controle de Bloqueio de Preços

[![.NET](https://img.shields.io/badge/.NET-8-blue?style=flat-square)](https://dotnet.microsoft.com/)
[![Visual Studio](https://img.shields.io/badge/Visual%20Studio-2022-blue?style=flat-square&logo=visual-studio&logoColor=white)](https://visualstudio.microsoft.com/)

---

## 📋 Sobre o projeto

A equipe comercial precisa garantir que determinados produtos não possam ter o preço alterado por questões estratégicas ou legais durante um período determinado.
O sistema Outlist gerencia essa lista de produtos bloqueados, garantindo que enquanto a vigência estiver ativa, alterações de preço sejam proibidas pelo sistema.

---

## 🚀 Funcionalidades

- 📥 **Adicionar** produtos à outlist com período de bloqueio  
- 🗑️ **Remover** produtos da outlist  
- 🕒 **Atualizar vigência** do bloqueio  
- 📄 **Listar** todos produtos (paginação com limite de 200 por página)  
- ✅ **Verificar** se um produto está bloqueado  

---

## 🛠️ Tecnologias

- C#  
- ASP.NET Core Web API  
- SpecFlow (BDD) para testes  
- xUnit para testes unitários  
- FluentAssertions para assertivas legíveis  
- HTTP Client para integração  

---

## 📦 Como rodar o projeto

1. Clone o repositório:  
   ```bash
   git clone https://github.com/LucimaraSouzah/outlist-repo.git
   cd outlist-repo

2. Configure o ambiente (ex: string de conexão, variáveis de ambiente)

3. Rode a API localmente:
Abra no Visual Studio e **execute a API (IIS Express)**

4. Para autenticação, utilize o token genérico abaixo no cabeçalho Authorization::
  ```
  Bearer meu-token-secreto

  ```

5. Execute os testes:
  ```
  dotnet test
  ```

## 📚 Documentação da API
POST /api/outlist - Adiciona produto à outlist

DELETE /api/outlist/{productId} - Remove produto da outlist

PUT /api/outlist/{productId} - Atualiza vigência do bloqueio

GET /api/outlist - Lista produtos bloqueados (paginado)

GET /api/outlist/check/{productId} - Verifica bloqueio de produto


## ✉️ Contato
Desenvolvido por Lucimara
