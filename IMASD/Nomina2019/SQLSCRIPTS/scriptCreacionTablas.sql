/*Script de creación de tablas*/

USE BDNOMINA2019;
GO

BEGIN TRANSACTION;  
  
BEGIN TRY  
    /*CREACION DE LA TABLA PADRON DE PERSONAS*/
		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblPadronPersonas')
		BEGIN
			CREATE TABLE tblPadronPersonas (
				iIdPadron int IDENTITY(1,1) NOT NULL PRIMARY KEY,
				cNombre varchar(255) NOT NULL,
				cApellido1 varchar(255),
				cApellido2 varchar(255),
				cDireccion varchar(255),
				cTelefono varchar(255),
				dtCreacion datetime,
				dtModificacion datetime
			);
			PRINT 'Se Creo correctamente la tabla tblPadronPersonas'
		END

		/*CREACION DE LA TABLA PARA LOS PERIODOS DE PAGO*/
		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblPeriodosPago')
		BEGIN
			CREATE TABLE tblPeriodosPago (
				iIdPeriodoPago int IDENTITY(1,1) NOT NULL PRIMARY KEY,
				cNombrePeriodo varchar(255),
				iDiasxPeriodo int,
				dtCreacion datetime,
				dtModificacion datetime
			);
			PRINT 'Se Creo correctamente la tabla tblPeriodosPago'
			/*SE CREAN LOS DATOS POR DEFAULT PARA PRUEBAS*/
			INSERT INTO tblPeriodosPago (cNombrePeriodo,iDiasxPeriodo,dtCreacion,dtModificacion)
			VALUES ('QUINCENAL',15,GETDATE(),GETDATE()),('MENSUAL',30,GETDATE(),GETDATE())
		END

		/*CREACION DE LA TABLA PARA LOS DEPARTAMENTOS*/
		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblDepartamentos')
		BEGIN
			CREATE TABLE tblDepartamentos (
				iIdDepartamento int IDENTITY(1,1) NOT NULL PRIMARY KEY,
				cNombreDepartamento varchar(255),
				dtCreacion datetime,
				dtModificacion datetime
			);
			PRINT 'Se Creo correctamente la tabla tblDepartamentos'
			/*SE CREAN LOS DATOS POR DEFAULT PARA PRUEBAS*/
			INSERT INTO tblDepartamentos (cNombreDepartamento,dtCreacion,dtModificacion)
			VALUES ('RH',GETDATE(),GETDATE()),('DESARROLLO',GETDATE(),GETDATE()),('CONTABILIDAD',GETDATE(),GETDATE())
		END

		/*CREACION DE LA TABLA PRINCIPAL PARA EMPLEADOS*/
		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblEmpleados')
		BEGIN
			CREATE TABLE tblEmpleados (
				iIdEmpleado int IDENTITY(1,1) NOT NULL PRIMARY KEY,
				iIdPadron int,
				iIdPeriodoPago int,
				iIdDepartamento int,
				lActivo bit,
				dtCreacion datetime,
				dtModificacion datetime,
				CONSTRAINT fk_Padron FOREIGN KEY (iIdPadron) REFERENCES tblPadronPersonas (iIdPadron),
				CONSTRAINT fk_PeriodoPago FOREIGN KEY (iIdPeriodoPago) REFERENCES tblPeriodosPago (iIdPeriodoPago),
				CONSTRAINT fk_Departamento FOREIGN KEY (iIdDepartamento) REFERENCES tblDepartamentos (iIdDepartamento)
			);
			PRINT 'Se Creo correctamente la tabla tblEmpleados'
		END

		/*CREACION DE LA TABLA PARA LOS TABULADORES DE SUELDO*/
		IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'tblTabuladorSueldo')
		BEGIN
			CREATE TABLE tblTabuladorSueldo (
				iIdTabuladorSueldo int IDENTITY(1,1) NOT NULL PRIMARY KEY,
				iIdEmpleado int,
				dSueldoDiario Decimal,
				lActivo bit,
				dtCreacion datetime,
				dtModificacion datetime,
				CONSTRAINT fk_Empleado FOREIGN KEY (iIdEmpleado) REFERENCES tblEmpleados (iIdEmpleado),
			);
			PRINT 'Se Creo correctamente la tabla tblTabuladorSueldo'
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
GO  