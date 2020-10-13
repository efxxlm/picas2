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
  public editable: boolean;
  public title;
  public contratoInfo;
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
        this.contratoInfo = data;
        
      });
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
    console.log(this.addressForm.value);
    this.openDialog('La información ha sido guardada exitosamente.', "");
  }

}
