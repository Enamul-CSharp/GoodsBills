using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCExam.Migrations
{
    /// <inheritdoc />
    public partial class fiftysp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			string SpInsertBill = @"CREATE or alter  PROCEDURE dbo.SpInsertBill
                   @BillDate datetime2(7),
                  @CustomerName nvarchar(max),  
                 @Address nvarchar(max),  
                  @ContactNo nvarchar(max)
AS
INSERT INTO [dbo].[Bills]
           ([BillDate]
           ,[CustomerName]
           ,[Address]
           ,[ContactNo])
     VALUES
           (@BillDate, @CustomerName, @Address, @ContactNo)

		   return @@identity 

GO";

            migrationBuilder.Sql(SpInsertBill);


            string SpInsertBillItem = @"CREATE or alter  PROCEDURE dbo.SpInsertBillItem
	@BillId int,
    @GoodsId int,
    @Quantity int
as

INSERT INTO [dbo].[BillItems]
           ([GoodsId]
           ,[Quantity]
           ,[BillId])
     VALUES
           (@GoodsId, @Quantity, @BillId )

		   return @@rowcount

GO";
            migrationBuilder.Sql(SpInsertBillItem);

            string SpUpdateBill = @"CREATE or alter PROCEDURE dbo.SpUpdateBill
    @BillId int,
    @BillDate datetime2(7),
    @CustomerName nvarchar(max),  
    @Address nvarchar(max),  
    @ContactNo nvarchar(max)
AS
UPDATE [dbo].[Bills]
   SET [BillDate] = @BillDate
      ,[CustomerName] = @CustomerName
      ,[Address] = @Address
      ,[ContactNo] = @ContactNo
	  where Id = @BillId

	  delete from BillItems where BillId = @BillId

	  return @@rowcount
GO";

            migrationBuilder.Sql(SpUpdateBill);


         string SpDeleteBill = @"CREATE or alter PROCEDURE [dbo].SpDeletebill
        @BillId int
       AS
	  delete from  BillItems where BillId = @BillId;
	   delete from [Bills] where Id = @BillId;

	  return @@rowcount
       GO";

            migrationBuilder.Sql(SpDeleteBill);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.Sql("drop proc SpInsertBill");
            migrationBuilder.Sql("drop proc SpInsertBillItem");
            migrationBuilder.Sql("drop proc SpUpdateBill");
            migrationBuilder.Sql("drop proc SpDeleteBill");
        }
    }
}


