import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-generar-acta-inicio-const-tecnico',
  templateUrl: './form-generar-acta-inicio-const-tecnico.component.html',
  styleUrls: ['./form-generar-acta-inicio-const-tecnico.component.scss']
})
export class FormGenerarActaInicioConstTecnicoComponent implements OnInit {

  maxDate: Date;
  maxDate2: Date;
  public idContrato;
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
  public editable: boolean;
  public title;

  public contratoCode;
  public fechaAprobacionSupervisor;
  public cantidadProyectosAsociados;
  public departamentoYMunicipioLlaveMEN;
  public fechaAprobacionGarantiaPoliza;
  public fechaFirmaContrato;
  public fechaGeneracionDRP1;
  public fechaGeneracionDRP2;
  public institucionEducativaLlaveMEN;
  public llaveMENContrato;
  public nombreEntidadContratistaObra;
  public nombreEntidadContratistaSupervisorInterventoria;
  public numeroContrato;
  public numeroDRP1;
  public numeroDRP2;
  public observacionOConsideracionesEspeciales;
  public plazoInicialContratoSupervisor;
  public valorActualContrato;
  public valorFase1Preconstruccion;
  public valorInicialContrato;
  public valorfase2ConstruccionObra;
  public vigenciaContrato;

  addressForm = this.fb.group({});
  dataDialog: {
    modalTitle: string,
    modalText: string
  };

  constructor(private router: Router, private activatedRoute: ActivatedRoute, public dialog: MatDialog, private fb: FormBuilder, private services: ActBeginService) {
    this.maxDate = new Date();
    this.maxDate2 = new Date();
  }
  ngOnInit(): void {
    this.addressForm = this.crearFormulario();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
    if(localStorage.getItem("editable")=="true"){
      this.editable=true;
      this.title='Ver detalle/Editar';
    }
    else{
      this.editable=false;
      this.title='Generar';
    }
  }
  loadData(id) {
      this.services.GetVistaGenerarActaInicio(id).subscribe(data=>{
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
        this.observacionOConsideracionesEspeciales = data.observacionOConsideracionesEspeciales;
        this.valorInicialContrato = data.valorInicialContrato;
        this.valorActualContrato = data.valorActualContrato;
        this.valorFase1Preconstruccion = data.valorFase1Preconstruccion;
        this.valorfase2ConstruccionObra = data.valorfase2ConstruccionObra;
        this.nombreEntidadContratistaSupervisorInterventoria = data.nombreEntidadContratistaSupervisorInterventoria;
        this.nombreEntidadContratistaObra = data.nombreEntidadContratistaObra;
        /*Campo de texto*/
      });
      this.idContrato = id;
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
      fechaActaInicioFDosConstruccion: [null, Validators.required],
      fechaPrevistaTerminacion: [null, Validators.required],
      mesPlazoEjFase2: [null, Validators.required],
      diasPlazoEjFase2: [null, Validators.required],
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
  removeTags(str){
    if ((str===null) || (str==='')){
      return false;
    }
    else{
      str = str.toString();
      return str.replace( /(<([^>]+)>)/ig, '');
    }
  }
  onSubmit() {
    this.removeTags(this.addressForm.value.observacionesEspeciales);
    this.services.CreatePlazoEjecucionFase2Construccion(this.idContrato,this.addressForm.value.mesPlazoEjFase2,this.addressForm.value.diasPlazoEjFase2,this.removeTags(this.addressForm.value.observacionesEspeciales),"usr2").subscribe(data1=>{
      if(data1.code=="102"){
        this.openDialog(data1.message,"");
        this.router.navigate(['/generarActaInicioConstruccion']);
      }
      else{
        this.openDialog(data1.message,"");
      }
    });
    console.log(this.addressForm.value);
    //this.openDialog('La información ha sido guardada exitosamente.', "");
  }

}
