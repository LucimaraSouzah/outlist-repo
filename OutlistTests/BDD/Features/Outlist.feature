Feature: Gerenciamento da Outlist
  Como usuário autorizado
  Eu quero gerenciar a Outlist
  Para controlar o bloqueio de preços de produtos

  Scenario: Adicionar um produto na Outlist
    Given que eu tenho um produto válido para adicionar
    When eu envio a requisição de adição
    Then o produto deve ser adicionado com sucesso

  Scenario: Remover um produto da Outlist
    Given que existe um produto adicionado
    When eu envio a requisição de remoção
    Then o produto deve ser removido com sucesso

  Scenario: Atualizar vigência de bloqueio de um produto
    Given que existe um produto adicionado
    When eu envio a requisição de atualização de vigência
    Then a vigência deve ser atualizada com sucesso

  Scenario: Buscar todos os produtos bloqueados
    When eu envio a requisição de listagem
    Then eu devo receber a lista de produtos bloqueados

  Scenario: Verificar se um produto está bloqueado
    Given que existe um produto adicionado
    When eu envio a requisição de verificação
    Then eu devo receber a confirmação de bloqueio
