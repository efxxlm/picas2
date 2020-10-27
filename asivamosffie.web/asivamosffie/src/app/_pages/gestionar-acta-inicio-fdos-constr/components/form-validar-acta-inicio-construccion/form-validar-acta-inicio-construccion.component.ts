import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ActBeginService, ContratoObservacion } from 'src/app/core/_services/actBegin/act-begin.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-validar-acta-inicio-construccion',
  templateUrl: './form-validar-acta-inicio-construccion.component.html',
  styleUrls: ['./form-validar-acta-inicio-construccion.component.scss']
})
export class FormValidarActaInicioConstruccionComponent implements OnInit {
  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };
  public editable: boolean;
  public title;

  public contratoId;
  contratoCode: string;
  fechaAprobacionSupervisor: Date;
  vigenciaContrato: Date;
  fechaFirmaContrato: Date;
  numeroDRP1: string;
  fechaGeneracionDRP1: Date;
  numeroDRP2: string;
  fechaGeneracionDRP2: Date;
  fechaAprobacionGarantiaPoliza: Date;
  observacionOConsideracionesEspeciales: string;
  valorInicialContrato: string;
  valorActualContrato: string;
  valorFase1Preconstruccion: string;
  valorfase2ConstruccionObra: string;
  nombreEntidadContratistaSupervisorInterventoria: string;
  nombreEntidadContratistaObra: string;
  fechaActaInicioConstruccion: any;
  fechaPrevistaTerminacion: any;
  obsConEspeciales: string;
  plazoActualContratoMeses: number;
  plazoActualContratoDias: number;
  plazoEjecucionPreConstruccionMeses: number;
  plazoEjecucionPreConstruccionDias: number;
  plazoEjecucionConstrM: number;
  plazoEjecucionConstrD: number;
  observacionID: any;

  fechaSesionString: string;
  fechaSesion: Date;
  fechaCreacion: Date;
  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder, private services: ActBeginService) { }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
      this.loadDataObservaciones(param.id);
    });
    this.loadConditionals();
  }

  loadConditionals(){
    if(localStorage.getItem("editable")=="true"){
      this.editable=true;
      this.title='Ver detalle/Editar';
    }
    else{
      this.editable=false;
      this.title='Validar';
    }
  }
  loadData(id) {
    this.services.GetVistaGenerarActaInicio(id).subscribe(data => {
      /*Titulo*/
      this.contratoCode = data.numeroContrato;
      this.fechaAprobacionSupervisor = data.plazoInicialContratoSupervisor;
      /*Cuadro 1*/
      this.vigenciaContrato = data.vigenciaContrato;
      this.fechaFirmaContrato = data.fechaFirmaContrato;
      this.numeroDRP1 = data.numeroDRP1;
      this.fechaGeneracionDRP1 = data.fechaGeneracionDRP1;
      this.numeroDRP2 = data.numeroDRP2;
      this.fechaGeneracionDRP2 = data.fechaGeneracionDRP2;
      this.fechaAprobacionGarantiaPoliza = data.fechaAprobacionGarantiaPoliza;
      this.observacionOConsideracionesEspeciales = data.objeto;
      this.valorInicialContrato = data.valorInicialContrato;
      this.valorActualContrato = data.valorActualContrato;
      this.valorFase1Preconstruccion = data.valorFase1Preconstruccion;
      this.valorfase2ConstruccionObra = data.valorfase2ConstruccionObra;
      this.nombreEntidadContratistaSupervisorInterventoria = data.nombreEntidadContratistaSupervisorInterventoria;
      this.nombreEntidadContratistaObra = data.nombreEntidadContratistaObra;
      /*Campo de texto no editable*/
      this.fechaActaInicioConstruccion = data.fechaActaInicioDateTime;
      this.fechaPrevistaTerminacion = data.fechaPrevistaTerminacionDateTime;
      this.obsConEspeciales = data.observacionOConsideracionesEspeciales;
      this.plazoActualContratoMeses = 12;
      this.plazoActualContratoDias = 26;
      this.plazoEjecucionPreConstruccionMeses = data.plazoFase1PreMeses;
      this.plazoEjecucionPreConstruccionDias = data.plazoFase1PreDias;
      this.plazoEjecucionConstrM = data.plazoFase2ConstruccionMeses;
      this.plazoEjecucionConstrD = data.plazoFase2ConstruccionDias;
    });
    this.contratoId = id;
  }
  loadDataObservaciones(id){
    if(localStorage.getItem("editable")=="true"){
      this.services.GetContratoObservacionByIdContratoId(id).subscribe(data0=>{
        this.addressForm.get('tieneObservaciones').setValue(data0.esActaFase2);
        this.addressForm.get('observaciones').setValue(data0.observaciones);
        this.loadIdObs(data0.contratoObservacionId);
        this.fechaCreacion = data0.fechaCreacion;
      });
    }
    else{
      this.services.GetContratoObservacionByIdContratoId(id).subscribe(data2=>{
        this.fechaCreacion = data2.fechaCreacion;
      });
    }
  }
  loadIdObs(id){
    this.observacionID = id;
  }
  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '25em',
      data: { modalTitle, modalText }
    });   
  }
  editorStyle = {
    height: '45px',
    overflow: 'auto'
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
      tieneObservaciones: [null, Validators.required],
      observaciones:['', Validators.required],
    })
  }
  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  generarActaSuscrita(){
    this.services.GetPlantillaActaInicio(this.contratoId).subscribe(resp=>{
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
  onSubmit() {
    this.fechaSesion = new Date(this.fechaCreacion);
    this.fechaSesionString = `${this.fechaSesion.getFullYear()}-${this.fechaSesion.getMonth() + 1}-${this.fechaSesion.getDate()}`;
    const contratoObs: ContratoObservacion ={
      contratoObservacionId:  this.observacionID,
      contratoId: this.contratoId,
      observaciones:  this.addressForm.value.observaciones,
      fechaCreacion: this.fechaSesionString,
      usuarioCreacion: "usr3",
      
      //opcionales
      esActa: true,
      fechaModificacion: this.fechaSesionString,
      usuarioModificacion: "usr3",
      esActaFase2: this.addressForm.value.tieneObservaciones
    };
    this.services.CreateEditContratoObservacion(contratoObs).subscribe(resp=>{
      if(resp.code=="200"){
        if(this.addressForm.value.tieneObservaciones==true){
          this.services.CambiarEstadoActa(this.contratoId,"16","usr2").subscribe(data0=>{
          
          });
        }
        else{
          this.services.CambiarEstadoActa(this.contratoId,"15","usr2").subscribe(data1=>{
          
          });
        }
        this.openDialog(resp.message, "");
        this.router.navigate(['/generarActaInicioConstruccion']);
      }
      else{
        this.openDialog(resp.message, "");
      }
    });
    console.log(this.addressForm.value);
    //this.openDialog('La información ha sido guardada exitosamente.', "");
  }

}
