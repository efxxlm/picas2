import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Respuesta } from 'src/app/core/_services/common/common.service';
import { RegisterProjectEtcService } from 'src/app/core/_services/registerProjectETC/register-project-etc.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ProyectoEntregaETC, RepresentanteETCRecorrido } from 'src/app/_interfaces/proyecto-entrega-etc';
import { FormRepresentanteComponent } from '../form-representante/form-representante.component';

@Component({
  selector: 'app-form-recorrido-obra',
  templateUrl: './form-recorrido-obra.component.html',
  styleUrls: ['./form-recorrido-obra.component.scss']
})
export class FormRecorridoObraComponent implements OnInit {

  @ViewChild(FormRepresentanteComponent ) childFormRepresentante: FormRepresentanteComponent ; 

  @Input() id: number;
  @Output("callOnInitParent") callOnInitParent: EventEmitter<any> = new EventEmitter();


  representanteEtcrecorrido: any;
  addressForm = this.fb.group({
    proyectoEntregaEtcid: [null, Validators.required],
    informeFinalId: [null, Validators.required],
    fechaRecorridoObra: [null, Validators.required],
    numRepresentantesRecorrido: [null, Validators.compose([Validators.required, Validators.maxLength(2)])],
    fechaFirmaActaEngregaFisica: [null, Validators.required],
    urlActaEntregaFisica: [null, Validators.required]
  });

  estaEditando = false;

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  constructor(private fb: FormBuilder, public dialog: MatDialog, private registerProjectETCService: RegisterProjectEtcService,private router: Router) {
  }


  ngOnInit(): void {
    this.buildForm();
  }

  
  private buildForm() {    
    this.addressForm = this.fb.group({
      proyectoEntregaEtcid: [null, Validators.required],
      informeFinalId: [this.id, Validators.required],
      fechaRecorridoObra: [null, Validators.required],
      numRepresentantesRecorrido: [null, Validators.compose([Validators.required, Validators.maxLength(2)])],
      fechaFirmaActaEngregaFisica: [null, Validators.required],
      urlActaEntregaFisica: [null, Validators.required]
    });

    if (this.id != null) {
      this.registerProjectETCService.getProyectoEntregaEtc(this.id)
      .subscribe(
        (response: ProyectoEntregaETC) => {
          if(response != null){
            this.representanteEtcrecorrido = response.representanteEtcrecorrido;
            this.addressForm.patchValue(response);
            console.log("response: ", response, this.representanteEtcrecorrido);
          }
        }
      );
      this.addressForm.markAllAsTouched();
      this.estaEditando = true;
    }
  }

  updateFromChild(){
    this.callOnInitParent.emit();
    this.ngOnInit();
    return;
  }

  arrayOne(n: number): any[] {
    return Array(n);
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.callOnInitParent.emit();
      this.ngOnInit();
      return;
    });
  }

  onSubmit() {
    this.addressForm.markAllAsTouched();
    this.estaEditando = true;
    this.addressForm.value.informeFinalId = this.id;

//recorre el datasource y crea modelo
    const proyectoEntregaEtcPartial = [] as ProyectoEntregaETC;
    proyectoEntregaEtcPartial.representanteEtcrecorrido = [];

    if(this.childFormRepresentante){
      if(this.childFormRepresentante.representantesForm.value != null){
        this.childFormRepresentante.representantesForm.value.representantes.forEach(value => {
          const representante: RepresentanteETCRecorrido = {
            representanteEtcid: value.representanteEtcid,
            proyectoEntregaEtcId: value.proyectoEntregaEtcId,
            nombre: value.nombre,
            cargo: value.cargo,
            dependencia:value.dependencia
          };
          proyectoEntregaEtcPartial.representanteEtcrecorrido.push(representante);
      });
      }
    }

    const proyectoEntregaEtc: ProyectoEntregaETC = {
      proyectoEntregaEtcId: this.addressForm.get('proyectoEntregaEtcid').value,
      informeFinalId: this.addressForm.get('informeFinalId').value,
      fechaRecorridoObra: this.addressForm.get('fechaRecorridoObra').value,
      numRepresentantesRecorrido: this.addressForm.get('numRepresentantesRecorrido').value,
      fechaFirmaActaEngregaFisica: this.addressForm.get('fechaFirmaActaEngregaFisica').value,
      urlActaEntregaFisica: this.addressForm.get('urlActaEntregaFisica').value,
      representanteEtcrecorrido: proyectoEntregaEtcPartial.representanteEtcrecorrido,
    };
    console.log("Modelo creado: ", proyectoEntregaEtc);
    this.createEditRecorridoObra(proyectoEntregaEtc);
  }

  createEditRecorridoObra(pRecorrido: any) {
    this.registerProjectETCService.createEditRecorridoObra(pRecorrido).subscribe((respuesta: Respuesta) => {
      this.openDialog('', respuesta.message);
    });
  }
  changeValueRepresentantes(event: any){
    console.log(event);
    this.addressForm.get("numRepresentantesRecorrido").setValue(event);
    this.childFormRepresentante.representantesForm.get( 'numRepresentantesRecorrido' ).setValue(event);
    return;
  }
}
