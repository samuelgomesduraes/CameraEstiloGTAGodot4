using Godot;
using System;
public partial class jogador : CharacterBody3D
{
	public const float Speed = 10.0f;
	public const float JumpVelocity = 4.5f;
	public float Gravity =20;
	public enum MachineState{IDLE,WALK,RUN,JUMP,FALL};
	MachineState Estado=MachineState.IDLE;
	public float MouseSensibilidade=0.05f;
	//BOOL
	public bool Andando=false;
	//NODES
	public Label HudState;
	public SpringArm3D Pivo;
	public AnimationPlayer animacao;
	public Marker3D Point;

	public override void _Ready(){
		HudState=GetNode<Label>("hud/state");
		Pivo=GetNode<SpringArm3D>("pivo");
		animacao=GetNode<AnimationPlayer>("textura/AnimationPlayer");
		Input.MouseMode=Input.MouseModeEnum.Captured;
	}
	public override void _Input(InputEvent @event){//rotacao do player e da camera
	if (@event is InputEventMouseMotion motionEvent){
		Vector3 rotDeg = RotationDegrees;
		rotDeg.Y-=motionEvent.Relative.X * MouseSensibilidade;
		RotationDegrees=rotDeg;
		rotDeg=Pivo.RotationDegrees;
		rotDeg.X-=motionEvent.Relative.Y * MouseSensibilidade;
		rotDeg.X=Mathf.Clamp(rotDeg.X,-90,0);
		Pivo.RotationDegrees=rotDeg;
	}
	}
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_cancel")){
			Input.MouseMode=Input.MouseModeEnum.Visible;
		}
		switch (Estado)
		{
			case MachineState.IDLE:IdleEstado();break;
			case MachineState.WALK:WalkEstado();break;
			case MachineState.JUMP:JumpEstado();break;
		}
		if(Input.IsActionJustPressed("shoot")){
			ColocarGelo();
		}
	}
	//ESTADOS
	private void IdleEstado(){
		HudState.Text="idle";
		animacao.Play("Idle");
		AplicarGravidade();
		
		if(Input.IsActionPressed("a") || Input.IsActionPressed("d") || Input.IsActionPressed("w") || Input.IsActionPressed("s")){
			Estado=MachineState.WALK;
		}
		else if(Input.IsActionPressed("space")){
			Estado=MachineState.JUMP;
		}
	}
	private void WalkEstado(){
		HudState.Text="walk";
		animacao.Play("Run");
		AplicarGravidade();
		Movimentacao();
		if(!Andando){
			Estado=MachineState.IDLE;
		}
	}
	private void JumpEstado(){
		HudState.Text="jump";
		animacao.Play("Jump");
		AplicarGravidade();

		if(IsOnFloor()){
			Estado=MachineState.IDLE;
		}
	}
	//PROPIEDADES
	private void AplicarGravidade(){
		Vector3 Motion=Velocity;
		Motion.Y+=-Gravity;
		Velocity=Motion;
		MoveAndSlide();
	}
	private void Movimentacao(){
		Vector3 velocity = Velocity;
		if(Input.IsActionPressed("w")){
			velocity=-Transform.Basis.Z * Speed;
			Andando=true;
		}
		else if(Input.IsActionPressed("s")){
			velocity=Transform.Basis.Z * Speed;
			Andando=true;
		}
		else if(Input.IsActionPressed("a")){
			velocity=-Transform.Basis.X * Speed;
			Andando=true;
		}
		else if(Input.IsActionPressed("d")){
			velocity=Transform.Basis.X * Speed;
			Andando=true;
		}
		else{
			velocity.Z=0;
			velocity.X=0;
			Andando=false;
		}
		Velocity=velocity;
		MoveAndSlide();
	}
	private void ColocarGelo(){
		var ParedeGel=(PackedScene)ResourceLoader.Load("res://assets/paredegel/paredeGel.tscn");
		Node3D ParedeNode =ParedeGel.Instantiate<Node3D>();
		GetTree().Root.AddChild(ParedeNode);
		ParedeNode.GlobalPosition=Point.GlobalPosition;
		GD.Print("linha 115 imcompleta,nome da funcao =colocar gelo");
	}
}
