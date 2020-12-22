import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSelectChange } from '@angular/material/select';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { DefensaJudicial } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-detalle-proceso-dj',
  templateUrl: './form-detalle-proceso-dj.component.html',
  styleUrls: ['./form-detalle-proceso-dj.component.scss']
})
export class FormDetalleProcesoDjComponent implements OnInit {
  addressForm = this.fb.group({
    departamentoInicio: [null, Validators.required],
    municipioInicio: [null, Validators.required],
    tipoAccion: [null, Validators.required],
    jurisdiccion: [null, Validators.required],
    pretensiones: [null, Validators.required],
    cuantiaPerjuicios: [null, Validators.required],
    requeridoParticipacionSupervisor: [null, Validators.required]
  });
  departamentoArray = [
  ];
  municipioArray = [
  ];
  tipoAccionArray = [
  ];
  jurisdiccionArray = [
  ];
  editorStyle = {
    height: '50px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  constructor(private fb: FormBuilder,public dialog: MatDialog, public commonService:CommonService) { }

  @Input() legitimacion:boolean;
  @Input() tipoProceso:string;
  @Input() defensaJudicial:DefensaJudicial;
  cargarRegistro() {
    //this.ngOnInit().then(() => {
      console.log("form");
      console.log(this.defensaJudicial);
      console.log(this.legitimacion);
      console.log(this.tipoProceso);      
  }
  
  ngOnInit(): void {
    this.commonService.listaDepartamentos().subscribe(response=>{
      this.departamentoArray=response;
    });

    this.commonService.listaTipoAccionJudicial().subscribe(response=>{
      this.tipoAccionArray=response;
    });

    this.commonService.listaJurisdiccion().subscribe(response=>{
      this.jurisdiccionArray=response;
    });
    
  }

  getMunicipio(event: MatSelectChange) {
    this.commonService.listaMunicipiosByIdDepartamento(event.value).subscribe(respuesta => {
      this.municipioArray = respuesta;
    },
      err => {
        let mensaje: string;
        console.log(err);
        if (err.message) {
          mensaje = err.message;
        }
        else if (err.error.message) {
          mensaje = err.error.message;
        }
        this.openDialog('Error', mensaje);
      },
      () => {
        // console.log('terminó');
      });
  }
  
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.openDialog('', 'La información ha sido guardada exitosamente.');
  }

  onChangeMun()
  {
    this.commonService.listaMunicipiosByIdDepartamento(this.addressForm.get("departamentoInicio").value).subscribe(response=>{
      this.municipioArray=response;
    });
  }
}
