select NewDifficulty,Count(*) 
from Level
group by NewDifficulty
order by NewDifficulty


select Difficulty,Count(*) 
from Level
group by Difficulty
order by Difficulty

select * from Level where Moves='(0,4)(4,2)(2,3)(3,0)(6,4)(5,5)(1,6)(4,4)'

select * from Level where IsUsed=1  order by NewDifficulty