import { Component, OnInit, ViewChild, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ContratistaGrilla, Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-tabla-resultados-contratistas',
  templateUrl: './tabla-resultados-contratistas.component.html',
  styleUrls: ['./tabla-resultados-contratistas.component.scss']
})
export class TablaResultadosContratistasComponent implements OnInit, OnChanges {

  @Input() contratacion: Contratacion;
  @Output() guardar: EventEmitter<any> = new EventEmitter();
  contratista: ContratistaGrilla;
  seDiligencioCampo = false;
  unionTemporal: FormControl;
  nombreContratista: FormControl;
  numeroDocumento: FormControl;

  displayedColumns: string[] = [
    'nombre',
    'nombreRepresentanteLegak',
    'numeroInvitacion',
    'numeroIdentificacion',
    'id'
  ];
  dataSource = new MatTableDataSource();

  edicion = false;

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: false}) sort: MatSort;

  elementosSelecciondos: any[] = [];

  constructor(
                private projectContractingService: ProjectContractingService,
                private dialog: MatDialog
             )
  {
    this.declararUnionTemporal();
  }
  ngOnChanges(changes: SimpleChanges): void {
    
    if ( changes.contratacion ){
      if (this.contratacion[ 'contratista' ] !== undefined)
        this.nombreContratista.setValue( this.contratacion[ 'contratista'].nombre );

      if (this.contratacion[ 'contratista' ] !== undefined)
         this.unionTemporal.setValue( this.contratacion[ 'contratista'].tipoProponenteCodigo === '4' ? true : false );
      
      if (this.contratacion[ 'contratista' ] !== undefined)
         this.numeroDocumento.setValue( this.contratacion[ 'contratista'].numeroIdentificacion );
      
      if ( this.contratacion[ 'contratista' ] !== undefined || this.contratacion[ 'contratista' ] !== undefined || this.contratacion[ 'contratista' ] !== undefined ) {
        this.buscar();
      }
    }

  }

  private declararUnionTemporal() {
    this.unionTemporal = new FormControl(null, Validators.required);
    this.nombreContratista = new FormControl('', Validators.required);
    this.numeroDocumento = new FormControl('', Validators.required);
  }

  ngOnInit(): void {

    setTimeout(() => {
      if ( this.contratacion[ 'contratista' ] !== undefined ) {
        this.contratista = {
          idContratista: this.contratacion.contratistaId,
        }
        this.edicion = true;
      }
    }, 2000);
  }

  selectElement(elemento: ContratistaGrilla) {
    this.contratista = {
      idContratista: elemento.idContratista,
    }
    this.seDiligencioCampo = true;
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  buscar(){

    if ( this.unionTemporal.value !== true && this.unionTemporal.value !== false ){
      this.openDialog( '', '<b>No se encontraron registros asociados al criterio de búsqueda seleccionado.</b>' );
      return false;
    }

    if (this.contratista)
      this.contratista.idContratista = 0;
    
    if (this.contratacion[ 'contratista' ] !== undefined)
      this.contratacion[ 'contratista' ].numeroIdentificacion = '';

    let nombre = this.nombreContratista.value;
    let numero = this.numeroDocumento.value;
    let esConsorcio = this.unionTemporal.value;
    this.dataSource = new MatTableDataSource();
    this.projectContractingService.getListContractingByFilters( numero, nombre, esConsorcio )
      .subscribe( response => {
        if ( response.length === 0 ) {
          this.openDialog( '', '<b>No se encontraron registros asociados al criterio de búsqueda seleccionado.</b>' );
          return;
        }



        this.dataSource = new MatTableDataSource(response);
        setTimeout(() => {
          this.dataSource.sort = this.sort;
          this.dataSource.paginator = this.paginator;
          this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
          this.paginator._intl.nextPageLabel = 'Siguiente';
          this.paginator._intl.previousPageLabel = 'Anterior';
        }, 10);
      })

  }

  checkRegistro( registro: any ) {
    if ( this.unionTemporal.value === false ) {
      if ( this.numeroDocumento.value !== null && this.nombreContratista.value !== null ) {
        if ( registro.numeroIdentificacion === this.numeroDocumento.value ) {
          return true;
        }
      }
    }

    if ( this.unionTemporal.value === true ) {
      if ( this.nombreContratista.value !== null ) {
        if ( this.nombreContratista.value === registro.nombre ) {
          return true;
        }
      }
    }
  }

  changeUnionTemporal(){
    this.nombreContratista.setValue('');
    this.numeroDocumento.setValue('');
    if (this.contratista)
      this.contratista.idContratista = 0;
    if (this.contratacion[ 'contratista' ] !== undefined)
      this.contratacion[ 'contratista' ].numeroIdentificacion = '';
    this.dataSource = new MatTableDataSource();
  }

  cargarRegistros(){

  }

  onSave(){
    console.log( this.contratista.idContratista )
    if (!this.contratista.idContratista || this.contratista.idContratista == 0)
    {
      this.openDialog('', 'No se ha seleccionado ningun contratista');
      return false;
    }
    this.contratacion.contratistaId = this.contratista.idContratista;
    this.guardar.emit(null);
  }

}
