import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ContratistaGrilla, Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';


@Component({
  selector: 'app-tabla-resultados-contratistas',
  templateUrl: './tabla-resultados-contratistas.component.html',
  styleUrls: ['./tabla-resultados-contratistas.component.scss']
})
export class TablaResultadosContratistasComponent implements OnInit {

  @Input() contratacion: Contratacion;
  @Output() guardar: EventEmitter<any> = new EventEmitter(); 

  contratista: ContratistaGrilla;

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

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  elementosSelecciondos: any[] = [];

  constructor(
                private projectContractingService: ProjectContractingService,

             ) 
  {
    this.declararUnionTemporal();
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
        this.dataSource = new MatTableDataSource( [ this.contratacion[ 'contratista' ] ] );
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
        this.paginator._intl.nextPageLabel = 'Siguiente';
        this.paginator._intl.previousPageLabel = 'Anterior';
      }
    }, 2000);

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  selectElement(elemento: ContratistaGrilla) {
    this.contratista = {
      idContratista: elemento.idContratista,

    }
  }

  buscar(){
    let nombre = this.nombreContratista.value;
    let numero = this.numeroDocumento.value;
    let esConsorcio = this.unionTemporal.value;
    this.dataSource = new MatTableDataSource();
    this.projectContractingService.getListContractingByFilters( numero, nombre, esConsorcio )
      .subscribe( response => {
        this.dataSource = new MatTableDataSource(response);
      })

  }

  cargarRegistros(){

  }

  onSave(){
    this.contratacion.contratistaId = this.contratista.idContratista;
    this.guardar.emit(null);
  }

}
