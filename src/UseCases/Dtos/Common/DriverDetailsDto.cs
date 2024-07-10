namespace UseCases.Dtos;
public class DriverDetailsDto
{

	public Nullable<int> KEYSEQUENCE { get; set; }

	public Nullable<DateTime> CREATIONDATETIME { get; set; }
	public string CREATIONUSERID { get; set; }
	public Nullable<DateTime> LASTUPDATEDATETIME { get; set; }
	public string LASTUPDATEUSERID { get; set; }
	public string COMPANYID { get; set; }

	public string DRIVER_ID { get; set; }
	public string PAYROLL_ID { get; set; }
	public string DRIVER_TYPE { get; set; }
	public Nullable<int> P_DRIVERCOMPANY { get; set; }
	public string FNAME { get; set; }

	public string MI { get; set; }
	public string LNAME { get; set; }
	public string ADDRESS { get; set; }
	public string City { get; set; }
	public string STATE { get; set; }

	public string ZIP { get; set; }
	public string PHONE1 { get; set; }
	public string PHONE2 { get; set; }
	public Nullable<DateTime> BIRTH_DATE { get; set; }
	public string SSNO { get; set; }

	public string FEDERAL_ID { get; set; }
	public Nullable<DateTime> HIRE_DATE { get; set; }
	public Nullable<DateTime> TER_DATE { get; set; }
	public string LIC_NO { get; set; }
	public string LIC_STATE { get; set; }

	public Nullable<DateTime> LIC_EXP_DATE { get; set; }
	public Nullable<DateTime> NEXTEXAM_DATE { get; set; }
	public string AUTHOR_STATES { get; set; }
	public string INS_CO_NAME { get; set; }
	public string INS_ID { get; set; }

	public string INS_TYPE { get; set; }
	public string STATUS { get; set; }
	public string NOTE { get; set; }
	public string TRUCK_MAKER { get; set; }
	public string TRUCK_MODEL { get; set; }

	public string TRUCK_VIN { get; set; }
	public string TRUCK_WEIGHT { get; set; }
	public string TYPE { get; set; }
	public string LICPLATE { get; set; }
	public string UNIFORM { get; set; }

	public string W_COMP { get; set; }
	public string LOAN_REPYT { get; set; }
	public string EARNINGS_ADJ { get; set; }
	public string INSURANCE { get; set; }
	public Nullable<Boolean> PayExclude { get; set; }

	public string DirectDep { get; set; }
	public string INSUR_OCC_ACC { get; set; }
	public string ELD_SEC_DEPOSIT { get; set; }
	public string ELD_USAGE_FEE { get; set; }
	public string NTLInsur { get; set; }

	public string UserID { get; set; }
}

