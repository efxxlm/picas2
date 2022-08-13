import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { FichaContratoService } from 'src/app/core/_services/fichaContrato/ficha-contrato.service';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-ficha-contrato',
  templateUrl: './ficha-contrato.component.html',
  styleUrls: ['./ficha-contrato.component.scss']
})
export class FichaContratoComponent implements OnInit {
  verResultados = false;
  mostrarFicha = false;
  contratosArray = [];
  numeroContratosArray = [];
  filterContratosArray: Observable<string[]>;
  addressForm: FormGroup = this.fb.group({
    numeroContrato: [null],
    nombreContratista: [null],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null],
    dede: [null],
    tipoContrato: [null],
    vigenciaContratación: [null]
  });

  listaTipoFicha = [
    {
      name: 'Ficha de contrato',
      value: 'Ficha de contrato'
    },
    {
      name: 'Ficha de proyecto',
      value: 'Ficha de proyecto'
    }
  ];
  resultados: any;
  indicadores = null;

  constructor(private fb: FormBuilder, private dialog: MatDialog, private fichaContratoService: FichaContratoService) {}

  ngOnInit(): void {
    this.getContratosByNumeroContrato();

    this.filterContratosArray = this.addressForm.get('numeroContrato').valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
  }

  _filter(value: string): string[] {
    const filterValue = value.toLowerCase();
  
    this.verResultados = false;
    this.mostrarFicha = false;

    return this.numeroContratosArray.filter(option => option.toLowerCase().indexOf(filterValue) === 0);
  }

  reiniciarFiltro() {
    this.addressForm.setValue({
      numeroContrato: '',
      nombreContratista: null,
      departamento: null,
      municipio: null,
      institucionEducativa: null,
      dede: null,
      tipoContrato: null,
      vigenciaContratación: null
    });

    this.verResultados = false;
    this.mostrarFicha = false;
  }
  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  getContratosByNumeroContrato() {
    this.fichaContratoService.getContratosByNumeroContrato('').subscribe(response => {
      this.contratosArray = response;

      for (let i = 0; i < this.contratosArray.length; i++) {
        const element = this.contratosArray[i];
        this.numeroContratosArray[i] = element.numeroContrato;
      }
      if (response.length === 0) {
        this.openDialog('', '<b>No se encontró un numero de contrato relacionado en la búsqueda.</b>');
      }
    });
  }

  getContratos(trigger: string) {
    if (trigger != null && trigger != undefined) {
      if (trigger.length >= 3) {
        /*this.limpiarListas(); */
        this.fichaContratoService.getContratosByNumeroContrato(trigger).subscribe(response => {
          this.contratosArray = response;
          if (response.length === 0) {
            this.openDialog('', '<b>No se encontró un numero de contrato relacionado en la búsqueda.</b>');
          }
        });
      }
    }
  }

  buscar() {
    if (this.addressForm.get('numeroContrato').value) {
      const pContratoId = this.contratosArray.find(
        e => e.numeroContrato === this.addressForm.get('numeroContrato').value
      ).contratoId;
      this.fichaContratoService.getFlujoContratoByContratoId(pContratoId).subscribe(response => {
        // console.log('getFlujoContratoByContratoId: ', response);
        this.resultados = response;
        this.verResultados = true;
      });
    }
    else {
      this.addressForm.markAllAsTouched(); 
      this.openDialog('', '<b>Número de contrato es un campo obligatorio en la búsqueda.</b>');
    }
  }

  onSubmit() {
    this.resultados = {};
    this.indicadores = null;
    this.buscar();
  }

  verFicha(event: any) {
    this.indicadores = null;

    if (event != null) {
      if (event?.contratoId > 0) {
        this.indicadores = this.resultados;
        this.mostrarFicha = event?.ficha;
      }
    }
  }
}
