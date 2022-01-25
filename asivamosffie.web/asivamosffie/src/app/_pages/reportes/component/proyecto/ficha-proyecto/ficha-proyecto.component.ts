import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { FichaProyectoService } from 'src/app/core/_services/fichaProyecto/ficha-proyecto.service';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-ficha-proyecto',
  templateUrl: './ficha-proyecto.component.html',
  styleUrls: ['./ficha-proyecto.component.scss']
})
export class FichaProyectoComponent implements OnInit {

  verResultados = false;
  mostrarFicha = false;

  addressForm: FormGroup = this.fb.group({
    proyectoId: [null, Validators.required],
    llaveMen: [null, Validators.required],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null],
    dede: [null],
    tipoIntervencion: [null],
    vigenciaContratacion: [null]
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
  ]

  proyectosArray = [];
  listDepartamento = [];
  listMunicipio = [];
  listInstEducativa = [];
  listSede = [];
  listTipoIntervencion = [];
  listVigencias = [];
  resultados = [];

  indicadores = null;

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private fichaProyectoService: FichaProyectoService,
    private commonSvc: CommonService
  ) { }

  ngOnInit(): void {
    this.fichaProyectoService.getVigencias().subscribe(response => this.listVigencias = response );
    this.commonSvc.listaTipoIntervencion().subscribe( response => this.listTipoIntervencion = response);
  }

  reiniciarFiltro() {
    this.addressForm.setValue({
      llaveMen: null,
      departamento: null,
      municipio: null,
      institucionEducativa: null,
      dede: null,
      tipoIntervencion: null,
      vigenciaContratacion: null,
      proyectoId: null
    });
    this.limpiarListas();
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  buscar() {
    this.fichaProyectoService.getTablaProyectosByProyectoIdTipoContratacionVigencia(
      this.addressForm.get('proyectoId').value,
      this.addressForm.get('tipoIntervencion').value ?? '',
      this.addressForm.get('vigenciaContratacion').value ?? 0
    ).subscribe(response => {
      this.resultados = response;
      this.verResultados = true;
    });
  }

  onSubmit() {
    this.resultados = [];
    this.indicadores = null;
    this.addressForm.markAllAsTouched();
    if ( this.addressForm.get( 'proyectoId' ).value !== null) {
      this.buscar();
    }else if(this.addressForm.get( 'proyectoId' ).value === null && this.addressForm.get( 'llaveMen' ).value !== null && this.addressForm.get( 'llaveMen' ).value !== ''){
      this.openDialog( '', '<b>Debe seleccionar una llave Men válida</b>' );
      return;
    }
  }

  verFicha(event: any) {
    this.indicadores = null;
    if(event != null){
      if(event?.proyectoId > 0){
        this.fichaProyectoService.getFlujoProyectoByProyectoId(event?.proyectoId)
        .subscribe(response => {
          this.indicadores = response;
          this.mostrarFicha = event?.ficha;
        });
      }
    }
  }

  getProyectos(trigger: string) {
    if(trigger != null && trigger != undefined){
      if(trigger.length >= 3){
        this.limpiarListas();
        this.fichaProyectoService.getProyectoIdByLlaveMen(trigger)
        .subscribe(response => {
          this.proyectosArray = response;
          if ( response.length === 0 ) {
            this.openDialog( '', '<b>No se encontró una Llave Men relacionada en la búsqueda.</b>' );
          }
        });
      }
    }
  }

  seleccionAutocomplete(proyecto: any) {
    this.limpiarListas();
    if(proyecto != null){
      this.addressForm.get('proyectoId').setValue(proyecto?.proyectoId);
      this.listDepartamento.push({
        value: proyecto?.departamento,
        name: proyecto?.departamento
      });
      this.listInstEducativa.push({
        value: proyecto?.institucionEducativa,
        name: proyecto?.institucionEducativa,
      });
      this.listMunicipio.push({
        value: proyecto?.municipio,
        name: proyecto?.municipio,
      });
      this.listSede.push({
        value: proyecto?.sede,
        name: proyecto?.sede,
      });
    }
  }

  limpiarListas(){
    this.listDepartamento = [];
    this.listMunicipio = [];
    this.listInstEducativa = [];
    this.listSede = [];
    this.addressForm.get('proyectoId').setValue(null);
    this.addressForm.get('vigenciaContratacion').setValue(null);
    this.addressForm.get('tipoIntervencion').setValue(null);
  }

}
