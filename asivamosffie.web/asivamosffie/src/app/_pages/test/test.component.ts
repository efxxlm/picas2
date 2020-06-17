import { Component, OnInit } from '@angular/core';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {
  perfiles: any[];

  constructor(private commonService: CommonService) {

  }

  ngOnInit(): void {
    this.commonService.loadProfiles().subscribe(dep => {
      console.log(dep);
      this.perfiles = dep;
    });
  }

}
