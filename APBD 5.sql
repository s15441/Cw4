create Procedure promote
	@studies nvarchar(100),
	@semester int
	as
	begin
	set XACT_ABORT ON
	BEGIN TRAN

	DECLARE @IdEnrollment INT, @ids INT, @newId INT;
	set @IdEnrollment = (select idEnrollment from enrollment where semester = @semester and idStudy = (select idstudy from studies where name = @studies));
	set @newId = (select idEnrollment from enrollment where semester = @semester+1 and idStudy = (select idstudy from studies where name = @studies));
	IF @newID is null 
	begin
		INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate)
		values (
		((select max (idenrollment) from enrollment)+1),
		(@semester+1),
		(select IdStudy s from studies where name = @studies), 
		getdate()
		);
		set @ids = (select max(IdEnrollment) from Enrollment);
	end
	else begin set @ids = @newId; end
		update student set IdEnrollment = @ids where IdEnrollment = @IdEnrollment;
	end