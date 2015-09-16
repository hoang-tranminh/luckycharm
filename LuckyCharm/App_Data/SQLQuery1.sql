--select top(100) * from (Select distinct(PrimaryNumber),[Date] from AnalysisItems order by [Date] desc)

sELECT * FROM AnalysisItems WHERE ID in
(Select B.ID FROM
( SELECT DISTINCT TOP 100 PrimaryNumber, [Date], A.RN, A.ID
FROM
(   SELECT PrimaryNumber, [Date],ID, ROW_NUMBER() over(partition by PrimaryNumber order by [Date] desc) RN
    FROM AnalysisItems
) A 
where A.RN =1
ORDER BY PrimaryNumber desc ) B ) Order by [Date] 

--select * from AnalysisItems where PrimaryNumber=N'28'