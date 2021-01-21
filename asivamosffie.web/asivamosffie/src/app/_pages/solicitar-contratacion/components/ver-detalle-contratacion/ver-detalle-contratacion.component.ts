import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from '../../../../core/_services/projectContracting/project-contracting.service';
import { CommonService, Dominio } from '../../../../core/_services/common/common.service';

@Component({
  selector: 'app-ver-detalle-contratacion',
  templateUrl: './ver-detalle-contratacion.component.html',
  styleUrls: ['./ver-detalle-contratacion.component.scss']
})
export class VerDetalleContratacionComponent implements OnInit {

  contratacion: Contratacion;
  aportanteVacio: boolean = false;
  ELEMENT_DATA    : any[]    = [
    { titulo: 'Tipo de intervención', name: 'tipoIntervencionCodigo' },
    { titulo: 'Llave MEN', name: 'llaveMen' },
    { titulo: 'Región', name: 'usuarioModificacion' },
    { titulo: 'Departamento/ Municipio', name: 'localizacionIdMunicipio' },
    { titulo: 'Institución Educativa', name: 'institucionEducativa' },
    { titulo: 'Sede', name: 'sede' }
  ];
  displayedColumns: string[] = [     
    'tipoIntervencionCodigo',
    'llaveMen',
    'usuarioModificacion',
    'localizacionIdMunicipio',
    'institucionEducativa',
    'sede' 
  ];
  fasesSelect: Dominio[] = [];
  componentesSelect: Dominio[] = [];
  usosSelect: Dominio[] = [];
  dataTable: any[] = [];
  dataAportantes: any[] = [];

  constructor ( private activatedRoute: ActivatedRoute,
                private projectContractingSvc: ProjectContractingService,
                private commonService: CommonService ) {
    console.log( this.activatedRoute.snapshot.params.id );
    this.getProjectContracting( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  }

  getProjectContracting ( id: any ) {
    this.projectContractingSvc.getContratacionByContratacionId( id )
      .subscribe( resp => {
        this.contratacion = resp;
        console.log( this.contratacion );
        for ( let contratacionProyecto of this.contratacion.contratacionProyecto ) {
          
          contratacionProyecto.dataAportantes = this.getDataAportantes( contratacionProyecto.contratacionProyectoAportante );

          this.dataTable.push(
            {
              tipoIntervencionCodigo: contratacionProyecto.proyecto.tipoIntervencionCodigo,
              llaveMen:contratacionProyecto.proyecto.llaveMen,
              usuarioModificacion: contratacionProyecto.proyecto.usuarioModificacion,
              localizacionIdMunicipio: contratacionProyecto.proyecto.localizacionIdMunicipio,
              institucionEducativa: contratacionProyecto.proyecto.institucionEducativa.nombre,
              sede: contratacionProyecto.proyecto.sede.nombre
            }
          )
        }
      } );
  }

  getDataAportantes ( contratacionProyecto: any ) {
    const aportante = [];
    forkJoin([
      this.commonService.listaFases(),
      this.commonService.listaComponentes(),
      this.commonService.listaUsos()
    ])
    .subscribe( resp => {
      this.fasesSelect = resp[0];
      this.componentesSelect = resp[1];
      this.usosSelect = resp[2];

      for ( let cont of contratacionProyecto ) {

        for ( let componente of cont.componenteAportante ) {

          let componenteSeleccionado = this.componentesSelect.filter( value => value.codigo === componente.tipoComponenteCodigo );
          let faseSeleccionado = this.fasesSelect.filter( value => value.codigo === componente.faseCodigo );

          for ( let uso of componente.componenteUso ) {

            let usoSeleccionado = this.usosSelect.filter( value => value.codigo === uso.tipoUsoCodigo );
            let apor = { 
              componente: componenteSeleccionado[0].nombre,
              fase: faseSeleccionado[0].nombre,
              tipoUso: [], 
              valorUso: [],
              contratacionProyectoAportanteId: cont.contratacionProyectoAportanteId,
              
            }
            apor.tipoUso.push( usoSeleccionado[0].nombre );
            apor.valorUso.push( uso.valorUso  );
            aportante.push( apor );
          };

        };

      };

    } );
    return aportante;
  };

};