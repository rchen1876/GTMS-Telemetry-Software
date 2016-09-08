
//RPM
//If under 2500, increase by 10 every 1 second
//if RPM reaches 3500, increase gear by 1, decrease rpm by 100 until rpm is about 2500
//if gear = 5, hold rpm at 2000 with a fluctuation of +/- 50

//Speed
//increase in speed is equal to 6/125 times increment rpm went up by
//If gear is 5 and speed is about 60, fluctuate with rpm

//BPFR, TP, O2, OP, FP, BAT- fluctuate by .1 just for kicks

//AT
//Rise up to 185

//LatG and Long G
//Deviate by +/-0.1

float BPFL = 12.32;
float BPFR = 11.28;
float BPRL = 12.24;
float BPRR = 12.21;

float OP = 18.3;
float O2 = 0.521;
int FP = 360;
float AT = 72.5;
float throttle = 450;

int RPM = 8000;
float Speed = 45;

float LatG = 0;
float LongG = 0;
int BAT = 1300;

int gear = 3;
float BPF = 0.00;
float BPR = 0.00;
float ET = 560;
float OT = 420;


int count = 0;
void setup()
{
	Serial.begin(9600);
}

void loop()
{
	

	Serial.println("ID 1885"); //RPM FP OP BAT

	Serial.print(":");
	Serial.println(RPM,1);

	Serial.print(":");
	Serial.println(Speed,1);

	Serial.print(":");
	Serial.println(throttle,1);

	Serial.print(":");
	Serial.println(BAT);
	delay(50);
	
	Serial.println("ID 1886");//O2 LongG RLWS LATG

	Serial.print(":");
	Serial.println(O2,1);

	Serial.print(":");
	Serial.println(gear,1);

	Serial.print(":");
	Serial.println(OP, 1);

	Serial.print(":");
	Serial.println(ET,1);
	delay(50);
	

	Serial.println("ID 1887");//AT Gear OP SteeringAngle

	Serial.print(":");
	Serial.println(FP,1);

	Serial.print(":");
	Serial.println(OT,1);

	Serial.print(":");
	Serial.println(OP);

	Serial.print(":");
	Serial.println(ET,1);
	delay(50);



	if (count % 10 == 0)
		OP += plusMinus()*0.1;
	else if (count % 15 == 0)
		FP += plusMinus()*0.1;
	else if (count % 20 == 0)
		O2 += plusMinus()*0.001;
	else if (count % 25 == 0)
		AT += plusMinus()*0.1;
	else if (count % 10 == 0)
		LatG += plusMinus()*0.01;
	else if (count % 10 == 0)
		LongG += plusMinus()*0.01;
	else if (count % 5 == 0) {
		Speed += plusMinus()*1;
		RPM += plusMinus()*10;
	}
	else if (count % 2 == 0) {
		LatG += plusMinus()*0.1;
		LongG += plusMinus()*0.1;
		BPF += plusMinus()*0.1;
		BPR += plusMinus()*0.1;
		BPFR += plusMinus()*0.1;
		BPFL += plusMinus()*0.1;
		BPRL += plusMinus()*0.1;
		BPRL += plusMinus()*0.1;
	}

	count++;
	if (count >= 1000)
	{
		count = 0;
	}
}

void printStuff(int count) {
	for (int i = 0; i < 4; i++)
	{
		Serial.print("Data:");
		Serial.println(count);
	}
}

int plusMinus() {
	int returnThis;
	
	if (random(100) >50)
	{
		returnThis = 1;
	}
	else {
		returnThis = -1;
	}
	return returnThis;
}
