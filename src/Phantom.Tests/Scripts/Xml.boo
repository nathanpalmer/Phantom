target foo:
	print "foo"

target xmlpeek_property:
	version_revision = xmlpeek('_revision.xml', '/info/entry/@revision', 'version.revision')
	print version_revision
	