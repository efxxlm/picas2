import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { Contrato, EditContrato, GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-ver-detalle-editar-acta-ini-f-i-prc',
  templateUrl: './ver-detalle-editar-acta-ini-f-i-prc.component.html',
  styleUrls: ['./ver-detalle-editar-acta-ini-f-i-prc.component.scss']
})
export class VerDetalleEditarActaIniFIPreconstruccioComponent implements OnInit {
  maxDate: Date;
  maxDate2: Date;
  public idContrato;
  public numContrato;
  public fechaFirmaContrato;
  public fechaContrato = "20/06/2020";//valor quemado
  public mesPlazoIni: number = 10;
  public diasPlazoIni: number = 25;
  public conObservaciones:boolean;

  public rolAsignado;
  public opcion;

  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public estadoDocumentoCodigo;
  public fechaEnvioFirma;
  public fechaFirmaContratista;
  public fechaFirmaFiduciaria;

  fechaSesionString: string;
  fechaSesion: Date;

  fechaSesionString2: string;
  fechaSesion2: Date;

  addressForm = this.fb.group({});
  addressForm2 = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  public observacionesOn: boolean;

  constructor(private router: Router,public dialog: MatDialog, private fb: FormBuilder, private activatedRoute: ActivatedRoute, private service: GestionarActPreConstrFUnoService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.addressForm2 = this.crearFormulario2();
    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.idContrato = param.id;
    });
  }
  loadData(id){
    this.service.GetContratoByContratoId(id).subscribe(data=>{
      this.cargarDataParaInsercion(data);
      this.verObservaciones(data.conObervacionesActa);
      //Datos correspondientes al formulario
      this.addressForm.get('fechaActaInicioFUnoPreconstruccion').setValue(data.fechaActaInicioFase1);
      this.addressForm.get('fechaPrevistaTerminacion').setValue(data.fechaTerminacion);
      this.addressForm.get('mesPlazoEjFase1').setValue(data.plazoFase1PreMeses);
      this.addressForm.get('diasPlazoEjFase1').setValue(data.plazoFase1PreDias);
      this.addressForm.get('mesPlazoEjFase2').setValue(data.plazoFase2ConstruccionMeses);
      this.addressForm.get('diasPlazoEjFase2').setValue(data.plazoFase2ConstruccionDias);
      this.addressForm.get('observacionesEspeciales').setValue(data.observaciones);
    });
    this.idContrato = id;
  }
  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 2) {
      this.opcion = 1;
    }
    else {
      this.opcion = 2;
    }
  }
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
  }
  generarActaSuscrita(){
    alert("genera PDf");
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '37em',
      data: { modalTitle, modalText }
    });
  }
  openDialog2(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });
  }
  editorStyle = {
    height: '50%'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  verObservaciones(observaciones){
    if(observaciones==true){
      this.conObservaciones=true;
    }
    else{
      this.conObservaciones=false;
    }
  }
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFUnoPreconstruccion: [Date(), Validators.required],
      fechaPrevistaTerminacion: [Date(), Validators.required],
      mesPlazoEjFase1: ["", Validators.required],
      diasPlazoEjFase1: ["", Validators.required],
      mesPlazoEjFase2: ["", Validators.required],
      diasPlazoEjFase2: ["", Validators.required],
      observacionesEspeciales: [""]
    })
  }
  crearFormulario2() {
    return this.fb.group({
      tieneObservaciones: ['', Validators.required],
      observaciones:[null, Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }
  number(e: { keyCode: any; }) {
    const tecla = e.keyCode;
    if (tecla === 8) { return true; } // Tecla de retroceso (para poder borrar)
    if (tecla === 48) { return true; } // 0
    if (tecla === 49) { return true; } // 1
    if (tecla === 50) { return true; } // 2
    if (tecla === 51) { return true; } // 3
    if (tecla === 52) { return true; } // 4
    if (tecla === 53) { return true; } // 5
    if (tecla === 54) { return true; } // 6
    if (tecla === 55) { return true; } // 7
    if (tecla === 56) { return true; } // 8
    if (tecla === 57) { return true; } // 9
    const patron = /1/; // ver nota
    const te = String.fromCharCode(tecla);
    return patron.test(te);
  }
  onSubmit() {
    let fecha = Date.parse(this.addressForm.get( 'fechaActaInicioFUnoPreconstruccion' ).value);
    this.fechaSesion = new Date(fecha);
    this.fechaSesionString = `${ this.fechaSesion.getFullYear() }-${ this.fechaSesion.getMonth() + 1 }-${ this.fechaSesion.getDate() }` 

    let fecha2 = Date.parse(this.addressForm.get( 'fechaPrevistaTerminacion' ).value);
    this.fechaSesion2 = new Date(fecha2);
    this.fechaSesionString2 = `${ this.fechaSesion2.getFullYear() }-${ this.fechaSesion2.getMonth() + 1 }-${ this.fechaSesion2.getDate() }` 

    if(this.addressForm.value.observacionesEspeciales!=""||this.addressForm.value.observacionesEspeciales!=null||this.addressForm.value.observacionesEspeciales!=undefined){
      this.observacionesOn=true;
    }
    else{
      this.observacionesOn=false;
    }
    //compara los meses
    var sumaMeses;
    var sumaDias;
    sumaMeses = parseInt(this.addressForm.value.mesPlazoEjFase1) + parseInt(this.addressForm.value.mesPlazoEjFase2);
    sumaDias = parseInt(this.addressForm.value.diasPlazoEjFase1) + parseInt(this.addressForm.value.diasPlazoEjFase2);
    if (sumaMeses > this.mesPlazoIni || sumaDias > this.diasPlazoIni) {
      this.openDialog('Debe verificar la información ingresada en el campo Plazo de ejecución - fase 1 - Preconstruccion Meses, dado que no coincide con la informacion inicial registrada para el contrato', "");
    }
    else {
      
      const arrayContrato: EditContrato = {
        contratoId: this.idContrato,
        contratacionId: this.contratacionId,
        fechaTramite: this.fechaTramite,
        tipoContratoCodigo: this.tipoContratoCodigo,
        numeroContrato: this.numContrato,
        estadoDocumentoCodigo: this.estadoDocumentoCodigo,
        estado: false,
        fechaEnvioFirma: this.fechaEnvioFirma,
        fechaFirmaContratista: this.fechaFirmaContratista,
        fechaFirmaFiduciaria: this.fechaFirmaFiduciaria,
        fechaFirmaContrato: this.fechaFirmaContrato,
        fechaActaInicioFase1: this.fechaSesionString,
        fechaTerminacion: this.fechaSesionString2,
        plazoFase1PreMeses: this.addressForm.value.mesPlazoEjFase1,
        plazoFase1PreDias: this.addressForm.value.diasPlazoEjFase1,
        plazoFase2ConstruccionMeses: this.addressForm.value.mesPlazoEjFase2,
        plazoFase2ConstruccionDias: this.addressForm.value.diasPlazoEjFase2,
        observaciones: this.addressForm.value.observacionesEspeciales,
        conObervacionesActa: this.observacionesOn,
        registroCompleto: false,
        contratoConstruccion: [],
        contratoObservacion: [],
        contratoPerfil: [],
        contratoPoliza: []
      };
      this.service.EditContrato(arrayContrato).subscribe(data => {
        this.openDialog('', data.message);
        if (data.code == "200") {
          this.router.navigate(['/generarActaInicioFaseIPreconstruccion']);
        }
      })
    }
    console.log(this.addressForm.value);
  }

}
