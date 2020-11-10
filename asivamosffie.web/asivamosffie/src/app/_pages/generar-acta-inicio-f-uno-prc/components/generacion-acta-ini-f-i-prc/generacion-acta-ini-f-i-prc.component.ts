import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { EditContrato, GestionarActPreConstrFUnoService } from 'src/app/core/_services/GestionarActPreConstrFUno/gestionar-act-pre-constr-funo.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-generacion-acta-ini-f-i-prc',
  templateUrl: './generacion-acta-ini-f-i-prc.component.html',
  styleUrls: ['./generacion-acta-ini-f-i-prc.component.scss']
})
export class GeneracionActaIniFIPreconstruccionComponent implements OnInit {
  maxDate: Date;
  maxDate2: Date;
  public idContrato;
  public numContrato;
  public fechaContrato = "20/06/2020";//valor quemado
  public fechaFirmaContrato;
  public contratacionId;
  public fechaTramite;
  public tipoContratoCodigo;
  public fechaEnvioFirma;
  public estadoDocumentoCodigo;
  public fechaFirmaContratista;
  public fechaFirmaFiduciaria;
  public mesPlazoIni: number = 10;
  public diasPlazoIni: number = 25;
  public observacionesOn : boolean;
  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  fechaAprobacionRequisitos: any;
  numDRP: any;
  fechaDRP: any;
  objeto: any;
  valorIni: any;
  nitContratistaInterventoria: any;
  nomContratista: any;

  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder, private service: GestionarActPreConstrFUnoService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
  }
  loadData(id) {
    this.service.GetContratoByContratoId(id).subscribe(data => {
      this.cargarDataParaInsercion(data);
    });
    this.idContrato = id;
  }
  cargarDataParaInsercion(data){
    this.numContrato = data.numeroContrato;
    this.fechaAprobacionRequisitos = data.fechaAprobacionRequisitos;
    this.fechaFirmaContrato = data.fechaFirmaContrato;
    this.contratacionId = data.contratacionId;
    this.fechaTramite = data.fechaTramite;
    this.tipoContratoCodigo = data.tipoContratoCodigo;
    this.estadoDocumentoCodigo = data.estadoDocumentoCodigo;
    this.fechaEnvioFirma = data.fechaEnvioFirma;
    this.fechaFirmaContratista = data.fechaFirmaContratista;
    this.fechaFirmaFiduciaria = data.fechaFirmaFiduciaria;
    this.numDRP = data.contratacion.disponibilidadPresupuestal[0].numeroDrp;
    this.fechaDRP = data.contratacion.disponibilidadPresupuestal[0].fechaCreacion;
    this.objeto = data.objeto;
    this.valorIni = data.valor;
    this.nitContratistaInterventoria = data.contratista.numeroIdentificacion;
    this.nomContratista = data.contratista.nombre;
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
  crearFormulario() {
    return this.fb.group({
      fechaActaInicioFUnoPreconstruccion: [null, Validators.required],
      fechaPrevistaTerminacion: [null, Validators.required],
      mesPlazoEjFase1: ["", Validators.required],
      diasPlazoEjFase1: ["", Validators.required],
      mesPlazoEjFase2: ["", Validators.required],
      diasPlazoEjFase2: ["", Validators.required],
      observacionesEspeciales: [null]
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
    if(this.addressForm.value.observacionesEspeciales!=""||this.addressForm.value.observacionesEspeciales!=null||this.addressForm.value.observacionesEspeciales!=undefined){
      this.observacionesOn=true;
    }
    else{
      this.observacionesOn=false;
    }
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
      fechaActaInicioFase1: this.addressForm.value.fechaActaInicioFUnoPreconstruccion,
      fechaTerminacion: this.addressForm.value.fechaPrevistaTerminacion,
      plazoFase1PreMeses: this.addressForm.value.mesPlazoEjFase1,
      plazoFase1PreDias: this.addressForm.value.diasPlazoEjFase1,
      plazoFase2ConstruccionMeses: this.addressForm.value.mesPlazoEjFase2,
      plazoFase2ConstruccionDias: this.addressForm.value.diasPlazoEjFase2,
      observaciones: this.addressForm.value.observacionesEspeciales,
      conObervacionesActa: this.observacionesOn,
      registroCompleto: true,
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
    console.log(this.addressForm.value);
    this.openDialog2('La información ha sido guardada exitosamente.', "");

  }
}
