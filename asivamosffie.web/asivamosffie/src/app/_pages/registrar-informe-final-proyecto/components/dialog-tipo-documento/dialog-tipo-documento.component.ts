import { Component, Inject, OnInit } from '@angular/core'
import { FormBuilder, Validators } from '@angular/forms'
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { Respuesta } from 'src/app/core/_services/common/common.service'
import { RegistrarInformeFinalProyectoService } from 'src/app/core/_services/registrarInformeFinal/registrar-informe-final-proyecto.service'
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component'
import { InformeFinalInterventoria } from 'src/app/_interfaces/proyecto-final-anexos.model';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-dialog-tipo-documento',
  templateUrl: './dialog-tipo-documento.component.html',
  styleUrls: ['./dialog-tipo-documento.component.scss']
})
export class DialogTipoDocumentoComponent implements OnInit {
  ELEMENT_DATA : InformeFinalInterventoria[] = [];
  anexos: any;
  dataSource = new MatTableDataSource<InformeFinalInterventoria>(this.ELEMENT_DATA);
  estaEditando = false;
  existe_historial = false;
  addressForm = this.fb.group({
    informeFinalAnexoId: [null, Validators.required],
    tipoAnexo: [null, Validators.required],
    urlSoporte: [null, Validators.required],
    numRadicadoSac: [null, Validators.compose([
      Validators.required, Validators.minLength(1), Validators.maxLength(10)])
    ],
    fechaRadicado: [null, Validators.required],
  })

  observacionesForm = this.fb.group({
    observaciones: [null, Validators.required],
    fechaCreacion: [null, Validators.required],   
  })

  tipoAnexoArray = [
    { name: 'Físico', value: '1' },
    { name: 'Digital', value: '2' }
  ]

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private registrarInformeFinalProyectoService: RegistrarInformeFinalProyectoService,
    @Inject(MAT_DIALOG_DATA) public data
  ) {}

  ngOnInit(): void {
    if(this.data.informeFinalAnexo != null){
      this.addressForm.patchValue(this.data.informeFinalAnexo)
    }else{
      this.getInformeFinalAnexoByInformeFinalInterventoriaId(this.data.informe.informeFinalInterventoriaId)
    }
  }

   // evalua tecla a tecla
   validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    })
  }

  onSubmit() {
    this.estaEditando = true;
    this.dialog.getDialogById('dialogTipoDocumento').close({ anexo: this.addressForm.value, id: this.data.informe.informeFinalInterventoriaId });
    this.openDialog('', '<b>La información ha sido guardada correctamente.</b>');
  }

  createEditInformeFinalAnexo(informeFinalAnexo: any, informeFinalInterventoriaid: number) {
    this.registrarInformeFinalProyectoService
      .createEditInformeFinalAnexo(informeFinalAnexo, informeFinalInterventoriaid)
      .subscribe((respuesta: Respuesta) => {
        this.openDialog('', respuesta.message)
        this.dialog.getDialogById('dialogTipoDocumento').close()
        return
      })
  }

  getInformeFinalAnexoByInformeFinalInterventoriaId(id: string) {
    this.registrarInformeFinalProyectoService.getInformeFinalAnexoByInformeFinalInterventoriaId(id).subscribe(anexos => {
        this.dataSource.data = anexos as InformeFinalInterventoria[];
        this.anexos = anexos;
        if(this.anexos.informeFinalAnexo != null){
          this.addressForm.patchValue(this.anexos.informeFinalAnexo)
        }
        if(this.anexos.observacionVigenteSupervisor != null){
          this.observacionesForm.patchValue(this.anexos.observacionVigenteSupervisor)
        }
        if(this.anexos.historialInformeFinalInterventoriaObservaciones != null){
          if(this.anexos.historialInformeFinalInterventoriaObservaciones.length > 0){
            this.existe_historial = true;
          }      
        }
    });
  }

  getInformeFinalAnexoByInformeFinalAnexoId(id: string) {
    this.registrarInformeFinalProyectoService.getInformeFinalAnexoByInformeFinalAnexoId(id).subscribe(responseData => {
      this.addressForm.patchValue(responseData)
    })
  }
}
