import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from '../../../../core/_services/projectContracting/project-contracting.service';

@Component({
  selector: 'app-ver-detalle-contratacion',
  templateUrl: './ver-detalle-contratacion.component.html',
  styleUrls: ['./ver-detalle-contratacion.component.scss']
})
export class VerDetalleContratacionComponent implements OnInit {

  contratacion: Contratacion;
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
  dataTable: any[] = [];


  constructor ( private activatedRoute: ActivatedRoute,
                private projectContractingSvc: ProjectContractingService ) {
    console.log( this.activatedRoute.snapshot.params.id );
    this.getProjectContracting( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getProjectContracting ( id: any ) {
    this.projectContractingSvc.getContratacionByContratacionId( id )
      .subscribe( resp => {
        this.contratacion = resp;
        console.log( this.contratacion );
        this.dataTable.push(
          {
            tipoIntervencionCodigo: this.contratacion.contratacionProyecto[0].proyecto.tipoIntervencionCodigo,
            llaveMen: this.contratacion.contratacionProyecto[0].proyecto.llaveMen,
            usuarioModificacion: this.contratacion.contratacionProyecto[0].proyecto.usuarioModificacion,
            localizacionIdMunicipio: this.contratacion.contratacionProyecto[0].proyecto.localizacionIdMunicipio,
            institucionEducativa: this.contratacion.contratacionProyecto[0].proyecto.institucionEducativa.nombre,
            sede: this.contratacion.contratacionProyecto[0].proyecto.sede.nombre
          }
        )
      } );
  }

}
