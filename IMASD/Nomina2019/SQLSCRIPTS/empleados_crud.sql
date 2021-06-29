CREATE PROCEDURE [dbo].[empleados_crud]
    @Id int = null,
	@IdPadron int = null,
	@cNombre varchar(MAX),
	@cApellido1 varchar(MAX),
	@cApellido2 varchar(MAX),
	@cDireccion varchar(MAX),
	@cTelefono varchar(MAX),
	@IdPeriodoPago int = null,
	@IdDepartamento int = null,
	@dSueldoDiario decimal = null,
	@Validar INT = 0
AS
BEGIN
   /*  
    Validar 1 --> Registra Un Empleado  
    Validar 2 --> Modifica Un Empleado  
	Validar 3 --> Inactiva Un Empleado
   */
BEGIN TRANSACTION;  
  
BEGIN TRY  
    SET NOCOUNT ON;
	/*Registrar un empleado*/
	IF @Validar = 1
	BEGIN
			/*SE CREA SU REGISTRO DE PADRON*/
			DECLARE @ULTIMOIDPADRON INT, @ULTIMOIDEMPLEADO INT
			INSERT INTO tblPadronPersonas(cNombre,cApellido1,cApellido2,cDireccion,cTelefono,dtCreacion,dtModificacion)
			VALUES (@cNombre,@cApellido1,@cApellido2,@cDireccion,@cTelefono,GETDATE(),GETDATE())
			SET @ULTIMOIDPADRON = IDENT_CURRENT('dbo.tblPadronPersonas')

			/*SE CREA SU REGISTRO DE EMPLEADO*/
			INSERT INTO tblEmpleados (iIdDepartamento,iIdPadron,iIdPeriodoPago,lActivo,dtCreacion,dtModificacion)
			VALUES (@IdDepartamento,@ULTIMOIDPADRON,@IdPeriodoPago,1,GETDATE(),GETDATE())
			SET @ULTIMOIDEMPLEADO = IDENT_CURRENT('dbo.tblEmpleados')

			/*SE CREA SU REGISTRO DE TABULADOR DE SUELDO*/
			INSERT INTO tblTabuladorSueldo (iIdEmpleado,dSueldoDiario,lActivo,dtCreacion,dtModificacion)
			VALUES (@ULTIMOIDEMPLEADO,@dSueldoDiario,1,GETDATE(),GETDATE())

			SELECT EMP.iIdEmpleado FROM tblEmpleados AS EMP WHERE iIdEmpleado = @ULTIMOIDEMPLEADO
	END

	/*Modificar un empleado*/
	IF @Validar = 2
	BEGIN
			/*SE MODIFICA SU REGISTRO DE PADRON*/
			DECLARE @FECHAACTUAL DATETIME
			SET @FECHAACTUAL = GETDATE()

			UPDATE tblPadronPersonas 
			SET cNombre = @cNombre,
				cApellido1 = @cApellido1,
				cApellido2 = @cApellido2,
				cDireccion = @cDireccion,
				cTelefono = @cTelefono,
				dtModificacion = @FECHAACTUAL
			where iIdPadron = @IdPadron


			/*SE CREA SU REGISTRO DE EMPLEADO*/
			UPDATE tblEmpleados 
			SET iIdDepartamento = @IdDepartamento,
				iIdPadron = @IdPadron,
				iIdPeriodoPago = @IdPeriodoPago,
				dtModificacion = @FECHAACTUAL
			WHERE iIdEmpleado = @Id

			UPDATE tblTabuladorSueldo SET lActivo = 0, dtModificacion = @FECHAACTUAL
			WHERE iIdEmpleado = @Id

			/*SE CREA SU REGISTRO DE TABULADOR DE SUELDO*/
			INSERT INTO tblTabuladorSueldo (iIdEmpleado,dSueldoDiario,lActivo,dtCreacion,dtModificacion)
			VALUES (@Id,@dSueldoDiario,1,GETDATE(),GETDATE())

			SELECT EMP.iIdEmpleado FROM tblEmpleados AS EMP WHERE iIdEmpleado = @Id

	END

	/*Inactivar un empleado*/
	IF @Validar = 3
	BEGIN
			IF @Id > 0
			BEGIN
				update tblEmpleados set lActivo = 0 where iIdEmpleado = @Id
				SELECT @Id
			END	
	END
END TRY  
BEGIN CATCH  
    SELECT   
        ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_LINE() AS ErrorLine  
        ,ERROR_MESSAGE() AS ErrorMessage;  
  
    IF @@TRANCOUNT > 0  
        ROLLBACK TRANSACTION;  
END CATCH;  
  
IF @@TRANCOUNT > 0  
    COMMIT TRANSACTION;  
  
END
