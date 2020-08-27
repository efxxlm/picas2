import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-proposiciones-varios',
  templateUrl: './form-proposiciones-varios.component.html',
  styleUrls: ['./form-proposiciones-varios.component.scss']
})
export class FormProposicionesVariosComponent implements OnInit {

  @Input() objetoComiteTecnico: ComiteTecnico 
  listaMiembros: Dominio[] = [];


  addressForm = this.fb.group({
    tema: this.fb.array([])
  });

  responsablesArray = [
    { name: 'reponsable 1', value: '1' },
    { name: 'reponsable 2', value: '2' },
    { name: 'reponsable 3', value: '3' }
  ];

  constructor(
              private fb: FormBuilder,
              public dialog: MatDialog,
              private commonService: CommonService,

             ) 
  {

  }

  ngOnInit(): void {
    this.commonService.listaMiembrosComiteTecnico()
      .subscribe( response => {
        this.listaMiembros = response;
      })
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  get tema() {
    return this.addressForm.get('tema') as FormArray;
  }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  borrarArray(borrarForm: any, i: number) {
    borrarForm.removeAt(i);
  }

  agregaTema() {
    this.tema.push(this.crearTema());
  }

  crearTema() {
    return this.fb.group({
      sesionTemaId: [],
      tema: [null, Validators.compose([
        Validators.required, Validators.minLength(5), Validators.maxLength(100)])
      ],
      responsable: [null, Validators.required],
      tiempoIntervencion: [null, Validators.compose([
        Validators.required, Validators.minLength(1), Validators.maxLength(3)])
      ],
      url: [null, [
        Validators.required,
      ]],
    });
  }

  onSubmit() {
    if (this.addressForm.valid) {
      this.openDialog(`La información ha sido guardada exitosamente`, '');
    }
  }

  cargarRegistros(){

    let lista = this.objetoComiteTecnico.sesionComiteTema.filter( t => t.esProposicionesVarios )

    lista.forEach( te => {
      let grupoTema = this.crearTema();
      let responsable = this.listaMiembros.find( m => m.codigo == te.responsableCodigo )

      grupoTema.get('tema').setValue( te.tema );
      grupoTema.get('responsable').setValue( responsable );
      grupoTema.get('tiempoIntervencion').setValue( te.tiempoIntervencion );
      grupoTema.get('url').setValue( te.rutaSoporte );
      grupoTema.get('sesionTemaId').setValue( te.sesionTemaId );


      this.tema.push( grupoTema )
    })

  }
}
