import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CommonService } from 'src/app/core/_services/common/common.service';
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
    { name: 'Reparacion Directa', value: '1' },
    { name: 'Reparacion Indirecta', value: '2' },
  ];
  jurisdiccionArray = [
    { name: 'Ordinaria', value: '1' },
    { name: 'Extraordinaria', value: '2' },
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

  ngOnInit(): void {
    this.commonService.listaDepartamentos().subscribe(response=>{
      this.departamentoArray=response;
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
