= let
    Username = "yourusername",
    Password = "pass",
    Base64Credentials = Binary.ToText(Text.ToBinary(Username & ":" & Password), BinaryEncoding.Base64),

    // Função para obter dados do Jira
    Jql = "project=TES",  // Modifique isso com a chave real do seu projeto
    Fields = "summary,key,issuetype,status,assignee,created",  // Adicione campos conforme necessário
    StartAt = "0",
    MaxR = "100",
    Url = "http://your-ip-or-domain:8080/rest/api/2/search?" & "jql=" & Jql & "&fields=" & Fields & "&maxResults=" & MaxR & "&startAt=" & StartAt,
    Source = Web.Contents(Url, 
        [ Headers = [ 
            Authorization = "Basic " & Base64Credentials,
            #"Content-Type" = "application/json"
        ]]
    ),
    JsonSource = Json.Document(Source),
    Issues = JsonSource[issues],

    // Transformar a lista de issues em uma tabela
    IssuesTable = Table.FromList(Issues, Splitter.SplitByNothing(), {"Column1"}, null, ExtraValues.Error),

    // Expandir as colunas para mostrar os campos relevantes
    ExpandedTable = Table.ExpandRecordColumn(IssuesTable, "Column1", {"key", "fields"}, {"Issue Key", "Fields"}),
    ExpandedFields = Table.ExpandRecordColumn(ExpandedTable, "Fields", {"summary", "issuetype", "status", "assignee", "created"}, {"Summary", "Issue Type", "Status", "Assignee", "Created"}),

    // Expansão adicional das colunas
    ExpandIssueType = Table.ExpandRecordColumn(ExpandedFields, "Issue Type", {"name"}, {"Issue Type"}),
    ExpandStatus = Table.ExpandRecordColumn(ExpandIssueType, "Status", {"name"}, {"Status"}),
    ExpandAssignee = Table.ExpandRecordColumn(ExpandStatus, "Assignee", {"displayName"}, {"Assignee"}),

    // Converter o campo Created para DateTimeZone
    ConvertCreatedToDateTimeZone = Table.TransformColumns(ExpandAssignee, {{"Created", each DateTimeZone.FromText(_), type datetimezone}}),

    // Adicionar a Data Atual para comparação
    CurrentDateTime = DateTimeZone.FixedLocalNow(),

    // Calcular a duração no status atual em meses e dias
    CalculateStatusDuration = Table.AddColumn(ConvertCreatedToDateTimeZone, "Status Duration", each Duration.From(CurrentDateTime - [Created])),
    FormatDuration = Table.AddColumn(CalculateStatusDuration, "Formatted Duration", each 
        let
            d = [Status Duration],
            days = Duration.Days(d),
            months = Number.RoundDown(days / 30),
            remainingDays = Number.Mod(days, 30),
            formatted = Text.Combine(List.Select({Text.From(months) & "m", Text.From(remainingDays) & "d"}, each Text.Length(_) > 1), " ")
        in
            formatted)
in
    FormatDuration
