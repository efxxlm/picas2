import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-registrar-controversia-contractua',
  templateUrl: './form-registrar-controversia-contractua.component.html',
  styleUrls: ['./form-registrar-controversia-contractua.component.scss']
})
export class FormRegistrarControversiaContractuaComponent implements OnInit {
  addressForm = this.fb.group({
    contrato: [null, Validators.required],
  });
  /*
  contratosArray = [
    { name: 'C223456789', value: '1' },
    { name: 'C223456999', value: '2' },
  ];
  */
  contratosArray:any;

  nombreContratista: any;
  tipoIdentificacion: any;
  numIdentificacion: any;
  tipoIntervencion: any;
  valorContrato: any;
  plazoContrato: any;
  fechaInicioContrato: any;
  fechaFinalizacionContrato: any;
  contratoId: number;

  constructor(  private fb: FormBuilder, public dialog: MatDialog, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.loadContractList();
  }

  loadContractList(){
    this.services.GetListContratos().subscribe(data=>{
      this.contratosArray = data;
    });
  }
  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  seleccionAutocomplete(id:any){
    this.addressForm.value.contrato = id;
    this.contratoId = id;
    this.services.GetVistaContratoContratista(id).subscribe(resp=>{
      this.nombreContratista = resp.nombreContratista;
      this.tipoIdentificacion = 'Pendiente de lectura del servicio';
      this.numIdentificacion = 'Pendiente de lectura del servicio';
      this.tipoIntervencion = 'Pendiente de lectura del servicio';
      this.valorContrato = 'Pendiente de lectura del servicio';
      this.plazoContrato = resp.plazoFormat;
      this.fechaInicioContrato = resp.fechaInicioContrato;
      this.fechaFinalizacionContrato = resp.fechaFinContrato;
    });
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
}
